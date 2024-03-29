﻿using VocabList.Core.DTOs.Identity;

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

        // Toplam rol sayısını döndürüyor..
        Task<int> GetTotalCountAsync();

        // Rolleri ve her rolün kaç kullanıcı içerdiği bilgisini bir liste şeklinde döndürüyor..
        Task<List<UserRolesCount>> GetUserRolesAsync();
    }
}
