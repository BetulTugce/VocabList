using AutoMapper;
using VocabList.Core.Entities;
using VocabList.Core.Repositories;
using VocabList.Core.Services;
using VocabList.Core.UnitOfWorks;

namespace VocabList.Service.Services
{
    public class EndpointService : Service<Endpoint>, IEndpointService
    {
        private readonly IEndpointRepository _endpointRepository;
        private readonly IMapper _mapper;

        public EndpointService(IGenericRepository<Endpoint> repository, IUnitOfWork unitOfWork, IEndpointRepository endpointRepository, IMapper mapper) : base(repository, unitOfWork)
        {
            _endpointRepository = endpointRepository;
            _mapper = mapper;
        }
    }
}
