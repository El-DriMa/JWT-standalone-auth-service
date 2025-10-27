using JWTAuthService.Models.Requests;
using JWTAuthService.Models.Responses;
using JWTAuthService.SearchObjects;

namespace JWTAuthService.Interfaces
{
    public interface IUserService : ICRUDService<UserResponse, UserSearchObject, UserInsertRequest, UserUpdateRequest>
    {
        Task ChangePassword(int userId, ChangePasswordRequest request);

    }

}
