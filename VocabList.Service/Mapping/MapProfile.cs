using AutoMapper;
using VocabList.Core.DTOs;
using VocabList.Core.Entities;

namespace VocabList.Service.Mapping
{
    public class MapProfile : Profile
    {
        public MapProfile() 
        { 
            CreateMap<WordList, WordListDto>().ReverseMap();
            CreateMap<Word, WordDto>().ReverseMap();
            CreateMap<Sentence, SentenceDto>().ReverseMap();
        }
    }
}
