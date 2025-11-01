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

            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).Build();
            var jwtAud = config.GetSection("Jwt:Audience").Value;
            var jwtIssuer = config.GetSection("Jwt:Issuer").Value;
            var jwtKey = config.GetSection("Jwt:Key").Value;

            var key = Encoding.ASCII.GetBytes(jwtKey!);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                [
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role,user.Role),
                ]),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Audience = jwtAud,
                Issuer = jwtIssuer,
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

      
    }
}
