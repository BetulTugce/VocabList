﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VocabList.Core.Services.Configurations;
using VocabList.Repository.Consts;
using VocabList.Repository.CustomAttributes;
using VocabList.Repository.Enums;

namespace VocabList.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Admin")]
    public class ApplicationServicesController : ControllerBase
    {
        readonly IApplicationService _applicationService;

        public ApplicationServicesController(IApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        [HttpGet]
        [AuthorizeDefinition(ActionType = ActionType.Reading, Definition = "Get Authorize Definition Endpoints", Menu = AuthorizeDefinitionConstants.ApplicationServices)]
        public IActionResult GetAuthorizeDefinitionEndpoints()
        {
            var datas = _applicationService.GetAuthorizeDefinitionEndpoints(typeof(Program));
            return Ok(datas);
        }
    }
}
