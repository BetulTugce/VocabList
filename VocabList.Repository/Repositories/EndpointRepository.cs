using VocabList.Core.Entities;
using VocabList.Core.Repositories;
using VocabList.Repository.Contexts;

namespace VocabList.Repository.Repositories
{
    public class EndpointRepository : GenericRepository<Endpoint>, IEndpointRepository
    {
        public EndpointRepository(VocabListDbContext context) : base(context)
        {

        }
    }
}
