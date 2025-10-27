using JWTAuthService.SearchObjects;

namespace JWTAuthService.Interfaces
{
    public interface ICRUDService<TModel, TSearch, TInsert, TUpdate> : IService<TModel, TSearch> where TModel : class where TSearch : BaseSearchObject where TInsert : class where TUpdate : class
    {
        Task<TModel> Insert(TInsert insert);
        Task<TModel> Update(int id, TUpdate request);
        Task<bool> Delete(int id);
    }

}
