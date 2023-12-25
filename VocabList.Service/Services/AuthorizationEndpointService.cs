using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VocabList.Core.Entities;
using VocabList.Core.Entities.Identity;
using VocabList.Core.Repositories;
using VocabList.Core.Services;
using VocabList.Core.Services.Configurations;
using VocabList.Core.UnitOfWorks;

namespace VocabList.Service.Services
{
    public class AuthorizationEndpointService : IAuthorizationEndpointService
    {
        readonly IApplicationService _applicationService;
        readonly RoleManager<AppRole> _roleManager;
        readonly IEndpointService _endpointService;
        readonly IEndpointRepository _endpointRepository;
        readonly IUnitOfWork _unitOfWork;
        readonly IMenuService _menuService;

        public AuthorizationEndpointService(IApplicationService applicationService, RoleManager<AppRole> roleManager, IEndpointService endpointService, IMenuService menuService, IUnitOfWork unitOfWork, IEndpointRepository endpointRepository)
        {
            _applicationService = applicationService;
            _roleManager = roleManager;
            _endpointService = endpointService;
            _menuService = menuService;
            _unitOfWork = unitOfWork;
            _endpointRepository = endpointRepository;
        }

        // Request ile gelen rolleri ilgili menunun altındaki code ile işaretlenmiş olan endpointe atar yani, endpointleri rollerle ilişkilendirir.
        public async Task AssignRoleEndpointAsync(string[] roles, string menu, string code, Type type)
        {
            Menu _menu = await _menuService.GetSingleAsync(m => m.Name == menu);
            if (_menu == null)
            {
                _menu = new()
                {
                    //Id = Guid.NewGuid(),
                    Name = menu
                };
                await _menuService.AddAsync(_menu);
            }

            // Code ve menu ile mevcut bir endpoint alınır veya yeni bir endpoint oluşturulur..
            Endpoint? endpoint = await _endpointRepository.GetAll().Include(e => e.Menu).Include(e => e.Roles).FirstOrDefaultAsync(e => e.Code == code && e.Menu.Name == menu);

            if (endpoint == null)
            {
                // Typea göre authorizeDefinition ile işaretlenmiş tüm endpointleri getirecek..
                var action = _applicationService.GetAuthorizeDefinitionEndpoints(type)
                        .FirstOrDefault(m => m.Name == menu)
                        ?.Actions.FirstOrDefault(e => e.Code == code);

                // Yeni bir endpoint oluşturur ve actiondan gelen bilgileri bu endpointe atar..
                endpoint = new()
                {
                    Code = action.Code,
                    ActionType = action.ActionType,
                    HttpType = action.HttpType,
                    Definition = action.Definition,
                    Menu = _menu
                };
                // Yeni oluşturulan endpoint veritabanına kaydedilir..
                await _endpointService.AddAsync(endpoint);
            }

            //foreach (var role in endpoint.Roles.ToList())
            //    endpoint.Roles.Remove(role);

            // endpointin ilişkilendirilmiş olduğu roller silinir ve veritabanında değişiklik güncellenir..
            endpoint.Roles.Clear();
            await _unitOfWork.CommitAsync();

            // Request ile gelen rollerin isim listesi kullanılarak rollerin listesi alınır..
            var appRoles = await _roleManager.Roles.Where(r => roles.Contains(r.Name)).ToListAsync();

            // endpoint ile roller ilişkilendirilir..
            foreach (var role in appRoles)
                endpoint.Roles.Add(role);

            // Değişiklikler veritabanına kaydedilir..
            await _unitOfWork.CommitAsync();
        }

        // Requestte gelen menunun altındaki code (endpointin unique değeri) ile ilişkilendirilmiş rolleri getirir..
        public async Task<List<string>> GetRolesToEndpointAsync(string code, string menu)
        {
            // Code ve menu ile endpoint var mı kontrol edilir..
            Endpoint? endpoint = await _endpointRepository.GetAll()
                .Include(e => e.Roles)
                .Include(e => e.Menu)
                .FirstOrDefaultAsync(e => e.Code == code && e.Menu.Name == menu);

            // Eğer endpoint varsa, rollerin adları alınır ve liste olarak döndürülür
            if (endpoint != null)
                return endpoint.Roles.Select(r => r.Name).ToList();
            // Yoksa null döner..
            return null;
        }
    }
}
