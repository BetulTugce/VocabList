using Microsoft.AspNetCore.Identity;
using VocabList.Core.Entities.Identity;
using VocabList.Core.Services;

namespace VocabList.Service.Services
{
    public class RoleService : IRoleService
    {
        readonly RoleManager<AppRole> _roleManager;
        public RoleService(RoleManager<AppRole> roleManager)
        {
            // Dependency Injection ile UserManager<AppUser> sınıfının bir örneğini bu servise enjekte eder.
            _roleManager = roleManager;
        }

        public async Task<bool> CreateRole(string name)
        {
            IdentityResult result = await _roleManager.CreateAsync(new() { Id = Guid.NewGuid().ToString(), Name = name });

            return result.Succeeded;
        }

        public async Task<bool> DeleteRole(string id)
        {
            AppRole appRole = await _roleManager.FindByIdAsync(id);
            IdentityResult result = await _roleManager.DeleteAsync(appRole);
            return result.Succeeded;
        }

        public (object, int) GetAllRoles(int page, int size)
        {
            var query = _roleManager.Roles;

            IQueryable<AppRole> rolesQuery = null;

            if (page != -1 && size != -1)
                rolesQuery = query.OrderBy(i => i.Name).Skip(page * size).Take(size);
            else
                rolesQuery = query;

            return (rolesQuery.OrderBy(i => i.Name).Select(r => new { r.Id, r.Name }), query.Count());
        }

        public async Task<(string id, string name)> GetRoleById(string id)
        {
            //string role = await _roleManager.GetRoleIdAsync(new() { Id = id });
            //return (id, role);
            AppRole appRole = await _roleManager.FindByIdAsync(id);
            if (appRole is not null)
            {
                return (appRole.Id, appRole.Name);
            }
            else
            {
                //throw new Exception("Rol bulunamadı!");
                return (null, null);
            }
        }

        public async Task<bool> HasRoleByNameAsync(string name)
        {
            AppRole appRole = await _roleManager.FindByNameAsync(name);
            if (appRole is not null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> UpdateRole(string id, string name)
        {
            AppRole role = await _roleManager.FindByIdAsync(id);
            role.Name = name;
            IdentityResult result = await _roleManager.UpdateAsync(role);
            return result.Succeeded;
        }
    }
}
