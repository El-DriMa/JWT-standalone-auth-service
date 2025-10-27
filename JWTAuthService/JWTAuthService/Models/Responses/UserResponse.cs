namespace JWTAuthService.Models.Responses
{
    public class UserResponse
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public DateTime? LastLoginAt { get; set; }
        public string Role { get; set; } = string.Empty;
    }

}
