using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc;
using VocabList.Core.Services;
using VocabList.Repository.CustomAttributes;
using System.Reflection;
using Microsoft.AspNetCore.Identity;
using VocabList.Core.Entities.Identity;

namespace VocabList.API.Filters
{
    public class RolePermissionFilter : IAsyncActionFilter
    {
        readonly IUserService _userService;
        readonly IConfiguration _configuration;
        //readonly IRoleService _roleService;

        public RolePermissionFilter(IUserService userService, IConfiguration configuration/*, IRoleService roleService*/)
        {
            _userService = userService;
            _configuration = configuration;
            //_roleService = roleService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            #region DefaultAdministratorEkleme
            // Burdaki işlemler daha sağlıklı olması için program.cse taşındı.
            //var user = await _userService.GetUserByNameorEmailAsync(_configuration["Administrator:Username"]);
            //if (user == null)
            //{
            //    if (!_roleService.HasRoleByNameAsync("Administrator").Result)
            //    {
            //        var createdRole = await _roleService.CreateRole("Administrator");
            //        if (createdRole)
            //        {
            //            var admin = await _userService.CreateAsync(new()
            //            {
            //                Email = _configuration["Administrator:Email"],
            //                Password = _configuration["Administrator:Password"],
            //                PasswordConfirm = _configuration["Administrator:Password"],
            //                Name = _configuration["Administrator:Name"],
            //                Surname = _configuration["Administrator:Surname"],
            //                Username = _configuration["Administrator:Username"]
            //            });
            //            if (admin != null)
            //            {
            //                string[] roles = { "Administrator" };
            //                await _userService.AssignRoleToUserAsnyc(admin.Id, roles);
            //            }
            //        }
            //    }
            //}
            #endregion

            // HttpContext üzerinden yapılan http ile bilgileri dönen bir instance dönüyor. Bu nesnenin içinden requestlere erişebilir, responselara müdahale edebilir ve userla ilgili çalışmaları gerçekleştirebiliriz..
            // İlgili kullanıcının username bilgisi alınır..
            var name = context.HttpContext.User.Identity?.Name;
            if (!string.IsNullOrEmpty(name) && name != _configuration["Administrator:Username"])
            { // name boş ya da null değilse..

                // İstek hangi actiona gitmek istiyorsa onunla ilgili bilgileri yakalar ancak actionın ismini vermiyor bu yüzden ControllerActionDescriptor türüne referans edilir..
                var descriptor = context.ActionDescriptor as ControllerActionDescriptor;

                // İlgili actionın işaretlenmiş olduğu AuthorizeDefinitionAttributeu yakalanır ve özellikleri okunur..
                // AuthorizeDefinitionAttribute türüne dönüştürülme sebebi, AuthorizeDefinitionAttributeun Attributedan türemiş olması ve dönüştürülmediği zaman AuthorizeDefinitionAttributeu kendi içerisindeki memberlarla görebilmek için kendi türüne referans etmek gerekli (polimorfizm :D)
                var attribute = descriptor.MethodInfo.GetCustomAttribute(typeof(AuthorizeDefinitionAttribute)) as AuthorizeDefinitionAttribute;

                // HttpMethodAttribute ile GET, POST, PUT vs. httpmethodun tür bilgisi alınır..
                var httpAttribute = descriptor.MethodInfo.GetCustomAttribute(typeof(HttpMethodAttribute)) as HttpMethodAttribute;

                // Elde edilen bilgilerle ilgili endpointin code bilgisi oluşturularak check edilebilecek..
                // httpAttribute işaretlenmemişse varsayılan olarak GET atanıyor..
                var code = $"{(httpAttribute != null ? httpAttribute.HttpMethods.First() : HttpMethods.Get)}.{attribute.ActionType}.{attribute.Definition.Replace(" ", "")}";

                // Endpointe atanan rollerden birisi ile kullanıcının sahip olduğu rollerden kesişen var mı kontrol ediliyor..
                var hasRole = await _userService.HasRolePermissionToEndpointAsync(name, code);

                // Rol yoksa yetkin yok..
                if (!hasRole)
                    context.Result = new UnauthorizedResult();
                else
                    await next(); // Sonraki middleware tetiklenir..
            }
            else // Yetki gerektirmeyen bir istek olacağı için sonraki fonksiyonu tetikler..
                await next();
        }
    }
}
