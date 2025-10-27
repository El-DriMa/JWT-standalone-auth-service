namespace JWTAuthService.Database
{
    public class Users : BaseEntity
    {
        public string Username { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string PasswordSalt { get; set; } = string.Empty;
        public DateTime? LastLoginAt { get; set; }
        public string Role { get; set; } = string.Empty;
       
    }
}
