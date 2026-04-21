using ContactListAPI.Data;
using ContactListAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ContactListAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ContactDbContext _context;
        private readonly IConfiguration _configuration;
        public AuthController(ContactDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        /// <summary>
        /// Służy do logowania użytkownika. Jeśli dane są poprawne, zwraca token JWT.
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto request )
        {
            // Szukaj urzytkownika po adresie email 
            var user = await _context.Contacts.FirstOrDefaultAsync(c => c.Email == request.Email);
            
            // Jezeli nie znaleziono emaila lub hasło się nie zgadza zwróć błąd
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password,user.Password))
            {
                return Unauthorized("Błędny adres email lub hasło.");
            }

            // Generowanie tokenu JWT
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email)
                }),
                Expires = DateTime.UtcNow.AddHours(2), // Token bedzie wazny przez 2 godziny
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // Zwracamy wygenerowany token do użytkownika
            return Ok(new { Token = tokenString });

        }
    }
}
