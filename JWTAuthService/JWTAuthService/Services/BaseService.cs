using MapsterMapper;
using JWTAuthService.Database;
using JWTAuthService.Interfaces;
using JWTAuthService.Models.Responses;
using JWTAuthService.SearchObjects;
using Microsoft.EntityFrameworkCore;

namespace JWTAuthService.Services
{
    public abstract class BaseService<TModel, TSearch, TDbEntity> : IService<TModel, TSearch> where TSearch : BaseSearchObject where TDbEntity : class where TModel : class
    {
        protected MyDatabaseContext _db { get; set; }
        protected IMapper Mapper { get; set; }
        protected BaseService(MyDatabaseContext db, IMapper mapper)
        {
            _db = db;
            Mapper = mapper;
        }

        public async Task<TModel> GetById(int id)
        {
            var entity = await _db.Set<TDbEntity>().FindAsync(id);
            if (entity != null)
            {
                return Mapper.Map<TModel>(entity);
            }
            else
            {
                return null;
            }
        }

        public virtual IQueryable<TDbEntity> AddFilter(TSearch search, IQueryable<TDbEntity> query)
        {
            return query;
        }
        public virtual async Task<PagedResult<TModel>> GetPaged(TSearch search)
        {
            var query = _db.Set<TDbEntity>().AsQueryable();

            query = AddFilter(search, query);

            int count = await query.CountAsync();

            if (search?.Page.HasValue == true && search?.PageSize.HasValue == true)
            {
                query = query.Skip((search.Page.Value - 1) * search.PageSize.Value).Take(search.PageSize.Value);
            }

            var list = await query.ToListAsync();

            var result = Mapper.Map<List<TModel>>(list);

            return new PagedResult<TModel>
            {
                Items = result,
                TotalCount = count
            };
        }
    }

}
