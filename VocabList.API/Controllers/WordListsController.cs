using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using VocabList.Core.DTOs;
using VocabList.Core.DTOs.Common;
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
            try
            {
                //WordList wordList = await _wordListService.GetByIdAsync(model.Id);
                //if (wordList == null)
                //{
                //    return NotFound(model);
                //}
                model.UpdatedDate = DateTime.UtcNow;
                var mappedData = _mapper.Map<WordList>(model);
                //await _wordListService.UpdateAsync(_mapper.Map<WordList>(model));
                bool response = await _wordListService.UpdateAsync(mappedData);
                if (response) { return Ok(); } else { return BadRequest(); }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpPost("[action]")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.WordLists, ActionType = ActionType.Reading, Definition = "Get All Word Lists By User Id")]
        public async Task<IActionResult> GetAllByUserId(GetAllWordListsByUserIdRequest request)
        {
            var response = await _wordListService.GetAllWordListsByUserIdAsync(request.Page, request.Size, request.AppUserId); //İlgili idye sahip kullanıcının kelime listelerini page ve sizea göre döner..
            if (!response.Any())
            {
                return NotFound();
            }
            var wordListsDtos = _mapper.Map<List<WordListDto>>(response); //Mapleme ile entityler dtoya çevriliyor..
            return Ok(wordListsDtos);
        }

        [HttpPost("[action]")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.WordLists, ActionType = ActionType.Reading, Definition = "Get Word List By Id And User Id")]
        public async Task<IActionResult> GetByIdAndUserId(GetWordListByIdAndUserIdRequest request)
        {
            var response = await _wordListService.GetWordListByIdAndUserIdAsync(request.Id, request.AppUserId); //İlgili idye ait datayı useridyi de kontrol ederek döner..
            if (response == null)
            {
                return NotFound(request.Id);
            }
            var wordListDto = _mapper.Map<WordListDto>(response); //Mapleme ile entity dtoya çevriliyor..
            return Ok(wordListDto);
        }

        // İlgili idye sahip listeyi getirir..
        [HttpGet("{id}")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.WordLists, ActionType = ActionType.Reading, Definition = "Get Word List By Id")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _wordListService.GetByIdAsync(id); //İlgili idye ait datayı döner..
            if (response == null)
            {
                return NotFound(id);
            }
            var wordListDto = _mapper.Map<WordListDto>(response); //Mapleme ile entity dtoya çevriliyor..
            return Ok(wordListDto);
        }

        // Parametredeki idye sahip listeyi siler..
        [HttpDelete("{id}")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.WordLists, ActionType = ActionType.Deleting, Definition = "Delete Word List By Id")]
        public async Task<IActionResult> Remove(int id)
        {
            //İlgili idye sahip data bulunuyor..
            var response = await _wordListService.GetByIdAsync(id);
            if (response == null)
            {
                return NotFound(id);
            }
            await _wordListService.RemoveAsync(response); //Data siliniyor..
            return NoContent();
        }

        // Oluşturulmuş tüm kelime listelerini siler..
        [HttpDelete]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.WordLists, ActionType = ActionType.Deleting, Definition = "Delete All Word Lists")]
        public async Task<IActionResult> RemoveAll() //TODO : Test edilecek..
        {
            var wordLists = await _wordListService.GetAllAsync(); //Tüm datayı döner..

            //Datalar siliniyor..
            await _wordListService.RemoveRangeAsync(wordLists);
            return NoContent();
        }

        //[HttpGet("GetTotalCount")]
        [HttpPost("[action]")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.WordLists, ActionType = ActionType.Reading, Definition = "Get Total Count By User Id")]
        public async Task<IActionResult> GetTotalCountByUserId(GetTotalCountByUserIdRequest request)
        {
            var wordListCount = await _wordListService.GetTotalCountByUserIdAsync(request.AppUserId); //Toplam liste sayısını döner..
            return Ok(wordListCount);
        }

        // Filtreleme seçeneklerine göre kelime listelerini içeren bir liste ve toplam kelime listesi sayısını döndürür..
        //[HttpGet]
        [HttpPost("[action]")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.WordLists, ActionType = ActionType.Reading, Definition = "Get Word Lists By Filter Options")]
        //public IActionResult GetFilteredWordLists([FromQuery] WordListFilterRequest filter, int pageNumber, int pageSize)
        public async Task<IActionResult> GetFilteredWordLists(WordListFilterRequest filter)
        {
            //var filteredWordLists = _wordListService.GetFilteredWordListsAsync(filter);
            //if (!filteredWordLists.Result.Item1.Any())
            //{
            //    return NotFound();
            //}
            
            //WordListFilterResponse response = new() { WordLists = _mapper.Map<List<WordListDto>>(filteredWordLists.Result.Item1), TotalCount = filteredWordLists.Result.Item2};
            //return Ok(response);

            // Filtreleme koşullarına uyan kelimele listelerini (page-size parametrelerine göre) ve bu koşulu sağlayan toplam kelime listesi sayısını getirir.. 
            var filteredWordLists = await _wordListService.GetFilteredWordListsAsync(filter);
            if (!filteredWordLists.Item1.Any())
            {
                return NotFound();
            }

            WordListFilterResponse response = new() { WordLists = _mapper.Map<List<WordListDto>>(filteredWordLists.Item1), TotalCount = filteredWordLists.Item2 };
            return Ok(response);

            //List<WordListDto> wordListsDtos = _mapper.Map<List<WordListDto>>(filteredWordLists.Result.Item1); //Mapleme ile entityler dtoya çevriliyor..
            //var result = new ObjectResult(new { WordLists = wordListsDtos, TotalCount = filteredWordLists.Result.Item2 })
            //{
            //    StatusCode = (int)HttpStatusCode.OK
            //};
            //return result;
        }
    }
}
