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
    public class SentencesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ISentenceService _sentenceService;

        public SentencesController(IMapper mapper, ISentenceService sentenceService)
        {
            _mapper = mapper;
            _sentenceService = sentenceService;
        }

        [HttpPost]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Sentences, ActionType = ActionType.Writing, Definition = "Create A New Sentence")]
        public async Task<IActionResult> Post(SentenceDto model)
        {
            await _sentenceService.AddAsync(_mapper.Map<Sentence>(model));
            return StatusCode((int)HttpStatusCode.Created);
        }

        // İlgili WordIdye sahip cümleleri getirir..
        [HttpGet("{id}")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Sentences, ActionType = ActionType.Reading, Definition = "Get All Sentences By WordId")]
        public async Task<IActionResult> GetAllByWordId(int id)
        {
            var response = await _sentenceService.GetAllByWordId(id); //İlgili WordIdye sahip cümleleri döner..
            if (response == null)
            {
                return NotFound(id);
            }
            var sentencesDto = _mapper.Map<List<SentenceDto>>(response); //Mapleme ile entity dtoya çevriliyor..
            return Ok(sentencesDto);
        }

        // Parametredeki idye sahip cümleyi siler..
        [HttpDelete("{id}")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Sentences, ActionType = ActionType.Deleting, Definition = "Delete Sentence By Id")]
        public async Task<IActionResult> Remove(int id)
        {
            //İlgili idye sahip data bulunuyor..
            var response = await _sentenceService.GetByIdAsync(id);
            if (response == null)
            {
                return NotFound(id);
            }
            await _sentenceService.RemoveAsync(response); //Data siliniyor..
            return NoContent();
        }
    }
}
