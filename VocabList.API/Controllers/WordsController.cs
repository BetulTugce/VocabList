using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using VocabList.Core.DTOs;
using VocabList.Core.Entities;
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
    public class WordsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IWordService _wordService;

        public WordsController(IMapper mapper, IWordService wordService)
        {
            _mapper = mapper;
            _wordService = wordService;
        }

        [HttpPost]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Words, ActionType = ActionType.Writing, Definition = "Create A New Word")]
        public async Task<IActionResult> Post(WordDto model)
        {
            await _wordService.AddAsync(_mapper.Map<Word>(model));
            return StatusCode((int)HttpStatusCode.Created);
        }

        [HttpPost("[action]")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Words, ActionType = ActionType.Reading, Definition = "Get All Words By User Id And Word List Id")]
        public async Task<IActionResult> GetAllByUserIdAndWordListId(GetAllWordsByUserIdAndWordListIdRequest request)
        {
            var response = await _wordService.GetAllWordsByUserIdAndWordListIdAsync(request.Page, request.Size, request.WordListId, request.AppUserId); //İlgili idye sahip kullanıcının kelimelerini page ve sizea göre döner..
            if (!response.Any())
            {
                return NotFound();
            }
            var wordListsDtos = _mapper.Map<List<WordDto>>(response); //Mapleme ile entityler dtoya çevriliyor..
            return Ok(wordListsDtos);
        }

        // Parametredeki idye sahip kelimeyi siler..
        [HttpDelete("{id}")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Words, ActionType = ActionType.Deleting, Definition = "Delete Word By Id")]
        public async Task<IActionResult> Remove(int id)
        {
            //İlgili idye sahip data bulunuyor..
            var response = await _wordService.GetByIdAsync(id);
            if (response == null)
            {
                return NotFound(id);
            }
            await _wordService.RemoveAsync(response); //Data siliniyor..
            return NoContent();
        }
    }
}
