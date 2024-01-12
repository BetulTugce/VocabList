namespace VocabList.Core.Services
{
    public interface IRoleService
    {
        Task<bool> CreateRole(string name);
        Task<bool> DeleteRole(string id);
        Task<bool> UpdateRole(string id, string name);
        (object, int) GetAllRoles(int page, int size);
        Task<(string id, string name)> GetRoleById(string id);
        Task<bool> HasRoleByNameAsync(string name);
        Task<int> GetTotalCountAsync();
    }
}
