using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using VocabList.Core.DTOs;
using VocabList.Core.Entities;
using VocabList.Core.Services;
using VocabList.Repository.Consts;
using VocabList.Repository.CustomAttributes;
using VocabList.Repository.Enums;

namespace VocabList.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Admin")]
    public class WordListsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IWordListService _wordListService;

        public WordListsController(IMapper mapper, IWordListService wordListService)
        {
            _mapper = mapper;
            _wordListService = wordListService;
        }

        [HttpPost]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.WordLists, ActionType = ActionType.Writing, Definition = "Create A New Word List")]
        public async Task<IActionResult> Post(WordListDto model)
        {
            await _wordListService.AddAsync(_mapper.Map<WordList>(model));
            return StatusCode((int)HttpStatusCode.Created);
        }

        [HttpPut]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.WordLists, ActionType = ActionType.Updating, Definition = "Update The Word List")]
        public async Task<IActionResult> Put(WordListDto model)
        {
            WordList wordList = await _wordListService.GetByIdAsync(model.Id);
            if (wordList == null)
            {
                return NotFound(model);
            }
            model.UpdatedDate = DateTime.Now;
            await _wordListService.UpdateAsync(_mapper.Map<WordList>(model));
            return Ok();
        }
    }
}
