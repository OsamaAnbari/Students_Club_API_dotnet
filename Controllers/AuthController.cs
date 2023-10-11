using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplication1.Models;
using WebApplication1.Services;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace WebApplication1.Controllers
{
    [Route("api/login")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserService userService;
        private readonly AdminService adminService;

        public AuthController(IConfiguration configuration)
        {
            userService = new UserService(configuration);
            adminService = new AdminService(configuration);
        }

        [HttpPost("admin")]
        public async Task<IActionResult> AdminLogin(Login model)
        {
            Admin auth = await adminService.AuthUsename(model);

            if (auth != null)
            {
                var token = GenerateJwtToken(auth.Name, "admin", auth.Id);
                return Ok(new { token });
            }

            return BadRequest(new {message = "Username or Password is wrong" });
        }

        [HttpPost("user")]
        public async Task<IActionResult> UserLogin(Login model)
        {
            User? auth = await userService.Login(model);

            if (auth != null)
            {
                var token = GenerateJwtToken(auth.Name, auth.Role, auth.Id);
                return Ok(new { token });
            }
            return BadRequest("Username or Password is wrong");
        }

        private string GenerateJwtToken(string name, string role, string id)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("7fb4895dcd29473f09bd3b9d1499246456dd1eda25daf3f66fd4c5bf990e257418e4d3");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, name),
                    new Claim(ClaimTypes.Role, role),
                    new Claim("userId", id)
                }),
                Expires = DateTime.UtcNow.AddHours(1), // Token expiration time
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        [HttpGet("login-google")]
        public IActionResult Login()
        {
            var redirectUrl = Url.Action(nameof(LoginCallback), "Login");
            var properties = new AuthenticationProperties
            {
                RedirectUri = redirectUrl
            };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet("login-callback")]
        public async Task<IActionResult> LoginCallback()
        {
            var authenticateResult = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (authenticateResult?.Succeeded == true)
            {
                // Authentication successful, you can handle the user here.
                // For demonstration, we're just redirecting to the home page.
                return RedirectToAction("Index", "Home");
            }

            return BadRequest("Google authentication failed.");
        }
    }
}