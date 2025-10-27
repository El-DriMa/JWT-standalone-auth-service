using JWTAuthService.Models.Responses;
using JWTAuthService.SearchObjects;

namespace JWTAuthService.Interfaces
{
    public interface IService<TModel, TSearch> where TSearch : BaseSearchObject
    {
        public Task<PagedResult<TModel>> GetPaged(TSearch search);
        public Task<TModel> GetById(int id);
    }

}
