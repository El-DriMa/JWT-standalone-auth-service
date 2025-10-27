namespace JWTAuthService.SearchObjects
{
    public class UserSearchObject : BaseSearchObject
    {
        public string? FTS { get; set; }
        public string? Username { get; set; }
        public string? Role {  get; set; }
    }
}
