using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using VocabList.Core.DTOs.Configuration;
using VocabList.Core.Services.Configurations;
using VocabList.Repository.CustomAttributes;
using VocabList.Repository.Enums;

namespace VocabList.Service.Configurations
{
    public class ApplicationService : IApplicationService
    {
        // Authorize gerektiren bütün endpointlerin bilgileri menu bazlı alınıyor..
        public List<Menu> GetAuthorizeDefinitionEndpoints(Type type)
        {
            Assembly assembly = Assembly.GetAssembly(type);

            // ControllerBase referansından türüyen tüm sınıfları getirir.
            var controllers = assembly.GetTypes().Where(t => t.IsAssignableTo(typeof(ControllerBase)));

            List<Menu> menus = new();
            if (controllers != null)
                foreach (var controller in controllers)
                { // Controllerların içine girerek GetMethods() ile her birinin içindeki methodları elde ediyoruz.. isDefined ile ilgili attributela işaretlenmiş olanları alıyoruz.. 
                    var actions = controller.GetMethods().Where(m => m.IsDefined(typeof(AuthorizeDefinitionAttribute)));
                    if (actions != null)
                        foreach (var action in actions)
                        {
                            var attributes = action.GetCustomAttributes(true);
                            if (attributes != null)
                            {
                                Menu menu = null;

                                var authorizeDefinitionAttribute = attributes.FirstOrDefault(a => a.GetType() == typeof(AuthorizeDefinitionAttribute)) as AuthorizeDefinitionAttribute;
                                // menus içerisinde authorizeDefinitionAttributeun içerisindeki menüye eşit bir değer yoksa..
                                if (!menus.Any(m => m.Name == authorizeDefinitionAttribute.Menu))
                                {
                                    // Menu oluşturuluyor..
                                    menu = new() { Name = authorizeDefinitionAttribute.Menu };
                                    // Oluşturulan menu koleksiyona ekleniyor..
                                    menus.Add(menu);
                                }
                                else // menus içerisinde authorizeDefinitionAttributeun içerisindeki menüye eşit bir değer varsa menuye atanıyor..
                                    menu = menus.FirstOrDefault(m => m.Name == authorizeDefinitionAttribute.Menu);

                                Core.DTOs.Configuration.Action _action = new()
                                {
                                    // Enum stringe dönüştürülerek ActionTypea atanıyor..
                                    ActionType = Enum.GetName(typeof(ActionType), authorizeDefinitionAttribute.ActionType),
                                    Definition = authorizeDefinitionAttribute.Definition
                                };

                                // HttpMethodAttribute attributedan kalıtım almışsa yani HttpGet, HttpPost vs. onları getirecek.. object olduğu için HttpMethodAttribute as edilerek dönüşümü sağlandı..
                                var httpAttribute = attributes.FirstOrDefault(a => a.GetType().IsAssignableTo(typeof(HttpMethodAttribute))) as HttpMethodAttribute;
                                if (httpAttribute != null)
                                    _action.HttpType = httpAttribute.HttpMethods.First();
                                else // Eğer bir httpAttribute yoksa default olarak HttpGet alıyor..
                                    _action.HttpType = HttpMethods.Get;

                                _action.Code = $"{_action.HttpType}.{_action.ActionType}.{_action.Definition.Replace(" ", "")}";

                                menu.Actions.Add(_action);
                            }
                        }
                }


            return menus;
        }
    }
}
