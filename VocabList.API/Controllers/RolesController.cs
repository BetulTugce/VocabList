using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VocabList.Core.DTOs.Identity;
using VocabList.Core.Services;
using VocabList.Repository.Consts;
using VocabList.Repository.CustomAttributes;
using VocabList.Repository.Enums;
using VocabList.Service.Services;

namespace VocabList.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Admin")]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        [AuthorizeDefinition(ActionType = ActionType.Reading, Definition = "Get Roles", Menu = AuthorizeDefinitionConstants.Roles)]
        public async Task<IActionResult> GetRoles([FromQuery] GetRolesQueryRequest getRolesQueryRequest)
        {
            try
            {
                var (datas, count) = _roleService.GetAllRoles(getRolesQueryRequest.Page, getRolesQueryRequest.Size);
                GetRolesQueryResponse response = new GetRolesQueryResponse();
                response.TotalCount = count;
                response.Datas = datas;
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"Internal Server Error: {ex.Message}" });
            }
        }

        [HttpPost()]
        [AuthorizeDefinition(ActionType = ActionType.Writing, Definition = "Create Role", Menu = AuthorizeDefinitionConstants.Roles)]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleRequest request)
        {
            try
            {
                var result = await _roleService.CreateRole(request.Name);
                //CreateRoleResponse response = new()
                //{
                //    Succeeded = result,
                //};
                //return Ok(response);
                if (result)
                {
                    return StatusCode(201, result);
                }
                else
                {
                    return BadRequest(new { Message = "İşlem başarısız, rol oluşturulamadı." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"Internal Server Error: {ex.Message}" });
            }
        }

        [HttpPut("{Id}")]
        [AuthorizeDefinition(ActionType = ActionType.Updating, Definition = "Update Role", Menu = AuthorizeDefinitionConstants.Roles)]
        public async Task<IActionResult> UpdateRole([FromBody, FromRoute] UpdateRoleRequest request)
        {
            try
            {
                var result = await _roleService.UpdateRole(request.Id, request.Name);
                if (result)
                {
                    return StatusCode(200, result);
                }
                else
                {
                    return StatusCode(400, new { Message = "İşlem başarısız, rol güncellenemedi." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"Internal Server Error: {ex.Message}" });
            }
        }

        [HttpDelete("{Id}")]
        [AuthorizeDefinition(ActionType = ActionType.Deleting, Definition = "Delete Role", Menu = AuthorizeDefinitionConstants.Roles)]
        public async Task<IActionResult> DeleteRole([FromRoute] DeleteRoleRequest request)
        {
            try
            {
                var result = await _roleService.DeleteRole(request.Id);
                if (result)
                {
                    //return StatusCode(200, result);
                    return Ok(result);
                }
                else
                {
                    return StatusCode(400, new { Message = "İşlem başarısız, rol silinemedi." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"Internal Server Error: {ex.Message}" });
            }
        }

        [HttpGet("{Id}")]
        [AuthorizeDefinition(ActionType = ActionType.Reading, Definition = "Get Role By Id", Menu = AuthorizeDefinitionConstants.Roles)]
        public async Task<IActionResult> GetRoles([FromRoute] GetRoleByIdQueryRequest request)
        {
            try
            {
                var data = await _roleService.GetRoleById(request.Id);
                if (!string.IsNullOrEmpty(data.id) && !string.IsNullOrEmpty(data.name))
                {
                    GetRoleByIdQueryResponse response = new()
                    {
                        Id = data.id,
                        Name = data.name,
                    };
                    return StatusCode(200, response);
                }
                else
                {
                    return StatusCode(404, new { Message = "İşlem başarısız, rol bilgisi alınamadı." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"Internal Server Error: {ex.Message}" });
            }
        }

        [HttpGet("[action]")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Roles, ActionType = ActionType.Reading, Definition = "Get Total Count")]
        public async Task<IActionResult> GetTotalCount()
        {
            //Toplam rol sayısını döner..
            return Ok(await _roleService.GetTotalCountAsync());
        }
    }
}
