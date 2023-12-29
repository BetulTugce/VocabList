using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VocabList.Core.DTOs.Identity;
using VocabList.Core.Services;
using VocabList.Repository.Consts;
using VocabList.Repository.CustomAttributes;
using VocabList.Repository.Enums;

namespace VocabList.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "Admin")]
    [ApiController]
    public class AuthorizationEndpointsController : ControllerBase
    {
        readonly IAuthorizationEndpointService _authorizationEndpointService;

        public AuthorizationEndpointsController(IAuthorizationEndpointService authorizationEndpointService)
        {
            _authorizationEndpointService = authorizationEndpointService;
        }

        // Requestte gelen menunun altındaki code (endpointin unique değeri) ile ilişkilendirilmiş rolleri getirir..
        [HttpPost("[action]")]
        [AuthorizeDefinition(ActionType = ActionType.Reading, Definition = "Get Roles To Endpoint", Menu = AuthorizeDefinitionConstants.AuthorizationEndpoints)]
        public async Task<IActionResult> GetRolesToEndpoint(GetRolesToEndpointQueryRequest request)
        {
            try
            {
                var datas = await _authorizationEndpointService.GetRolesToEndpointAsync(request.Code, request.Menu);
                GetRolesToEndpointQueryResponse response = new()
                {
                    Roles = datas
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"Internal Server Error: {ex.Message}" });
            }
            
        }

        // Request ile gelen rolleri ilgili menunun altındaki code ile işaretlenmiş olan endpointe atar yani, endpointleri rollerle ilişkilendirir.
        [HttpPost]
        [AuthorizeDefinition(ActionType = ActionType.Writing, Definition = "Assign Role To Endpoint", Menu = AuthorizeDefinitionConstants.AuthorizationEndpoints)]
        public async Task<IActionResult> AssignRoleEndpoint(AssignRoleEndpointRequest request)
        {
            try
            {
                await _authorizationEndpointService.AssignRoleEndpointAsync(request.Roles, request.Menu, request.Code, typeof(Program));
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"Internal Server Error: {ex.Message}" });
            }
        }
    }
}
