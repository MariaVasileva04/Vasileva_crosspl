using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Vasileva_crosspl.Models;
using Microsoft.AspNetCore.Authorization;

namespace Vasileva_crosspl.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var user = new User { Login = request.Login };

            user.Password = request.Password;

            bool isValid = (request.Login == "admin" && request.Password == "admin123") ||
                           (request.Login == "user" && request.Password == "user123");

            if (!isValid)
                return Unauthorized(new { message = "Неверный логин или пароль" });

            var token = AuthOptions.GenerateToken(user.IsAdmin);
            return Ok(token);

        }
    }
    public class LoginRequest
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
