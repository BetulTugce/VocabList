using VocabList.Core.DTOs.Configuration;

namespace VocabList.Core.Services.Configurations
{
    public interface IApplicationService
    {
        // Uygulamada AuthorizeDefinition attributeu ile işaretlenmiş tüm endpointleri getirecek..
        List<Menu> GetAuthorizeDefinitionEndpoints(Type type);
    }
}
