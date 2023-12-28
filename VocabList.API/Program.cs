using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using VocabList.API.Filters;
using VocabList.Core.Authentications;
using VocabList.Core.Entities.Identity;
using VocabList.Core.Repositories;
using VocabList.Core.Services;
using VocabList.Core.Services.Configurations;
using VocabList.Core.UnitOfWorks;
using VocabList.Repository.Contexts;
using VocabList.Repository.Repositories;
using VocabList.Repository.Token;
using VocabList.Repository.UnitOfWorks;
using VocabList.Service.Configurations;
using VocabList.Service.Mail;
using VocabList.Service.Mapping;
using VocabList.Service.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<VocabListDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), options =>
    {
        options.MigrationsAssembly(Assembly.GetAssembly(typeof(VocabListDbContext)).GetName().Name);
    });
});

builder.Services.AddIdentity<AppUser, AppRole>().AddEntityFrameworkStores<VocabListDbContext>().AddDefaultTokenProviders();
builder.Services.AddAutoMapper(typeof(MapProfile));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped(typeof(IService<>), typeof(Service<>));
builder.Services.AddScoped<IWordListRepository, WordListRepository>();
builder.Services.AddScoped<IWordListService, WordListService>();
builder.Services.AddScoped<IWordRepository, WordRepository>();
builder.Services.AddScoped<IWordService, WordService>();
builder.Services.AddScoped<ISentenceRepository, SentenceRepository>();
builder.Services.AddScoped<ISentenceService, SentenceService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IInternalAuthentication, AuthService>();
builder.Services.AddScoped<ITokenHandler, VocabList.Service.Token.TokenHandler>();
builder.Services.AddScoped<IMailService, MailService>();
builder.Services.AddScoped<IApplicationService, ApplicationService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IMenuRepository, MenuRepository>();
builder.Services.AddScoped<IMenuService, MenuService>();
builder.Services.AddScoped<IEndpointRepository, EndpointRepository>();
builder.Services.AddScoped<IEndpointService, EndpointService>();
builder.Services.AddScoped<IAuthorizationEndpointService, AuthorizationEndpointService>();

builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>
    policy.WithOrigins("http://localhost:4200", "https://localhost:4200").AllowAnyHeader().AllowAnyMethod().AllowCredentials()
));
builder.Services.AddControllers(options =>
{
    options.Filters.Add<RolePermissionFilter>();
});
// Uygulamaya token �zerinden bir istek gelirse token� do�rularken JWT oldu�unu bilecek ve buradaki konfig�rasyonlar �zerinden token� do�rulayacak.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(builder.Configuration["Administrator:Role"], options =>
    {
        // JWT ile ilgili temel konfig�rasyonlar..
        options.TokenValidationParameters = new()
        {
            // Gelen tokenda do�rulanacak de�erleri bildiriliyor..
            ValidateAudience = true, // Olu�turulacak token de�erini kimlerin/hangi originlerin(sitelerin) kullanaca��n� belirledi�imiz de�er. -> www.vocablist.com
            ValidateIssuer = true, // Olu�turulacak token de�erini kimin da��tt���n� ifade edece�imiz alan. -> www.vocablistapi.com
            ValidateLifetime = true, // Olu�turulan token de�erinin s�resini kontrol edecek olan do�rulama. S�resi ge�mi�se authorize etmeyecek.
            ValidateIssuerSigningKey = true, // �retilecek token de�erinin uygulamam�za ait bir de�er oldu�unu ifade eden security key verisinin do�rulanmas�.

            ValidAudience = builder.Configuration["Token:Audience"],
            ValidIssuer = builder.Configuration["Token:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"])),
            LifetimeValidator = (notBefore, expires, securityToken, validationParameters) => expires != null ? expires > DateTime.UtcNow : false,

            NameClaimType = ClaimTypes.Name // JWT �zerinde Name claime kar��l�k gelen de�eri User.Identity.Name propertysinden elde edebiliriz.
        };
    });

var app = builder.Build();

// API aya�a kalkarken appsettings.jsondaki Administrator alt�ndaki Emaile sahip kullan�c� yoksa default olarak eklenip Administrator rol� atan�yor..
using (var serviceScope = app.Services.CreateScope())
{
    var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
    var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();
    var user = userManager.FindByEmailAsync(builder.Configuration["Administrator:Email"]).Result;
    if (user == null)
    {
        IdentityResult result = await userManager.CreateAsync(new()
        {
            Id = Guid.NewGuid().ToString(),
            Email = builder.Configuration["Administrator:Email"],
            Name = builder.Configuration["Administrator:Name"],
            Surname = builder.Configuration["Administrator:Surname"],
            UserName = builder.Configuration["Administrator:Username"],
        }, builder.Configuration["Administrator:Password"]);

        if (result.Succeeded)
        {
            AppUser? defaultUser = await userManager.FindByEmailAsync(builder.Configuration["Administrator:Email"]);
            if (roleManager.FindByNameAsync(builder.Configuration["Administrator:Role"]).Result == null)
            {
                AppRole role = new AppRole();
                role.Id = Guid.NewGuid().ToString();
                role.Name = builder.Configuration["Administrator:Role"];

                var createdRole = roleManager.CreateAsync(role).Result;
                if (createdRole.Succeeded)
                {
                    userManager.AddToRoleAsync(defaultUser, builder.Configuration["Administrator:Role"]).Wait();
                }
            }
        }
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
