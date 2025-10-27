using JWTAuthService.Database;
using JWTAuthService.Helpers;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JWTAuthService.Services
{
    public class AuthService(MyDatabaseContext db)
    {
        public async Task<string?> LoginAsync(string username, string password)
        {
            var user = db.Users.FirstOrDefault(u => u.Username == username);
            if (user == null) return null;

            var isPasswordValid = PasswordHelper.VerifyPassword(password, user.PasswordHash, user.PasswordSalt);
            if (!isPasswordValid) return null;

            user.LastLoginAt = DateTime.Now;
            await db.SaveChangesAsync();

            var token = GenerateJwtToken(user);
            return token;
        }
        public static string GenerateJwtToken(Users user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("bc4cb666fb5816f66b3c463d4f3ed80f");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                [
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                ]),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Audience = "https://localhost:7284/",
                Issuer = "https://localhost:7284/"
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

      
    }
}
