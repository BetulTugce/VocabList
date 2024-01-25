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

        [HttpPut]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Words, ActionType = ActionType.Updating, Definition = "Update The Word")]
        public async Task<IActionResult> Put(WordDto model)
        {
            try
            {
                model.UpdatedDate = DateTime.UtcNow;
                var mappedData = _mapper.Map<Word>(model);
                bool response = await _wordService.UpdateAsync(mappedData);
                if (response) { return Ok(); } else { return BadRequest(); }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
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

        [HttpPost("[action]")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Words, ActionType = ActionType.Reading, Definition = "Get Total Count By WordList Id")]
        public async Task<IActionResult> GetTotalCountByWordListId(GetTotalCountByWordListIdRequest request)
        {
            var wordCount = await _wordService.GetTotalCountByWordListIdAsync(request.WordListId); //Toplam kelime sayısını döner..
            return Ok(wordCount);
        }

        [HttpPost("[action]")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Words, ActionType = ActionType.Reading, Definition = "Get Words By Filter Options")]
        public async Task<IActionResult> GetFilteredWords(WordFilterRequest filter)
        {
            // Filtreleme koşullarına uyan kelimeleri (page-size parametrelerine göre) ve bu koşulu sağlayan toplam kelime sayısını getirir.. 
            var filteredWords = await _wordService.GetFilteredWordsAsync(filter.SearchString, filter.Page, filter.Size, filter.Sort, filter.OrderBy, filter.WordListId, filter.AppUserId);
            if (!filteredWords.Item1.Any())
            {
                return NotFound();
            }

            WordFilterResponse response = new() { Words = _mapper.Map<List<WordDto>>(filteredWords.Item1), TotalCount = filteredWords.Item2 };
            return Ok(response);
        }
    }
}
