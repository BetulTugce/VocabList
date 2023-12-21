using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;
using VocabList.Core.Authentications;
using VocabList.Core.Entities.Identity;
using VocabList.Core.Repositories;
using VocabList.Core.Services;
using VocabList.Core.UnitOfWorks;
using VocabList.Repository.Contexts;
using VocabList.Repository.Repositories;
using VocabList.Repository.Token;
using VocabList.Repository.UnitOfWorks;
using VocabList.Service.Mail;
using VocabList.Service.Mapping;
using VocabList.Service.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
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

builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>
    policy.WithOrigins("http://localhost:4200", "https://localhost:4200").AllowAnyHeader().AllowAnyMethod().AllowCredentials()
));

// Uygulamaya token üzerinden bir istek gelirse tokený doðrularken JWT olduðunu bilecek ve buradaki konfigürasyonlar üzerinden tokený doðrulayacak.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer("Admin", options =>
    {
        // JWT ile ilgili temel konfigürasyonlar..
        options.TokenValidationParameters = new()
        {
            // Gelen tokenda doðrulanacak deðerleri bildiriliyor..
            ValidateAudience = true, // Oluþturulacak token deðerini kimlerin/hangi originlerin(sitelerin) kullanacaðýný belirlediðimiz deðer. -> www.vocablist.com
            ValidateIssuer = true, // Oluþturulacak token deðerini kimin daðýttýðýný ifade edeceðimiz alan. -> www.vocablistapi.com
            ValidateLifetime = true, // Oluþturulan token deðerinin süresini kontrol edecek olan doðrulama. Süresi geçmiþse authorize etmeyecek.
            ValidateIssuerSigningKey = true, // Üretilecek token deðerinin uygulamamýza ait bir deðer olduðunu ifade eden security key verisinin doðrulanmasý.

            ValidAudience = builder.Configuration["Token:Audience"],
            ValidIssuer = builder.Configuration["Token:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"])),
            LifetimeValidator = (notBefore, expires, securityToken, validationParameters) => expires != null ? expires > DateTime.UtcNow : false,
        };
    });

var app = builder.Build();

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
