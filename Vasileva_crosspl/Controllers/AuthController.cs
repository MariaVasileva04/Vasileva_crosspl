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
            string adminpas = "admin123";
            string usepas = "user123";

            string adminhash = ComputeMd5(adminpas);
            string usehash = ComputeMd5(usepas);

            bool isValid = false;
            if (request.Login == "admin" && request.Password == adminhash)
                isValid = true;
            else if (request.Login == "user" && request.Password == usehash)
                isValid = true;

            if (!isValid)
                return Unauthorized(new { message = "Неверный логин или пароль" });

            var user = new User { Login = request.Login};
            var token = AuthOptions.GenerateToken(user.IsAdmin);
            return Ok(token);

        }
        private string ComputeMd5(string input)
        {
            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                var inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                var hashBytes = md5.ComputeHash(inputBytes);
                return Convert.ToHexString(hashBytes).ToLowerInvariant();
            }
        }


    }
    public class LoginRequest
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }

    }
