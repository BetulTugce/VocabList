namespace VocabList.Core.Services
{
    public interface IAuthorizationEndpointService
    {
        // Request ile gelen rolleri ilgili menunun altındaki code ile işaretlenmiş olan endpointe atar yani, endpointleri rollerle ilişkilendirir.
        public Task AssignRoleEndpointAsync(string[] roles, string menu, string code, Type type);

        // Requestte gelen menunun altındaki code (endpointin unique değeri) ile ilişkilendirilmiş rolleri getirir..
        public Task<List<string>> GetRolesToEndpointAsync(string code, string menu);
    }
}
