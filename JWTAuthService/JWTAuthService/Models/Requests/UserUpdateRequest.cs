using System.ComponentModel.DataAnnotations;

namespace JWTAuthService.Models.Requests
{
    public class UserUpdateRequest
    {

        [MaxLength(50, ErrorMessage = "First name can't be more than 50 characters.")]
        public string? FirstName { get; set; } = string.Empty;


        [MaxLength(50, ErrorMessage = "Last name can't be more than 50 characters.")]
        public string? LastName { get; set; } = string.Empty;

        public string? Password { get; set; } = string.Empty;
        public string? Role { get; set; } = string.Empty;

    }

}
