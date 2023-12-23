using AutoMapper;
using VocabList.Core.Entities;
using VocabList.Core.Repositories;
using VocabList.Core.Services;
using VocabList.Core.UnitOfWorks;
using VocabList.Repository.Repositories;

namespace VocabList.Service.Services
{
    public class MenuService : Service<Menu>, IMenuService
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IMapper _mapper;

        public MenuService(IGenericRepository<Menu> repository, IUnitOfWork unitOfWork, IMenuRepository menuRepository, IMapper mapper) : base(repository, unitOfWork)
        {
            _menuRepository = menuRepository;
            _mapper = mapper;
        }
    }
}
