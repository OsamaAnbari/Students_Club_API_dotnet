using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost("login")]
        public IActionResult Login(LoginModel model)
        {
            // Authenticate user (replace this with your authentication logic)
            // If authentication is successful, generate a JWT token
            var token = GenerateJwtToken();

            return Ok(new { token });
        }

        private string GenerateJwtToken()
        {
            // Generate JWT token (replace with your token generation logic)
            // Use JwtSecurityToken and JwtSecurityTokenHandler
            // You can set claims, expiration, and other properties of the token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("7fb4895dcd29473f09bd3b9d1499246456dd1eda25daf3f66fd4c5bf990e257418e4d3");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                new Claim(ClaimTypes.Name, "username"),
                    // Add additional claims as needed
                }),
                Expires = DateTime.UtcNow.AddHours(1), // Token expiration time
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }

    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
