using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Resto_Backend.Data;
//using Resto_Backend.Middleware;
using Resto_Backend.Model;
using Resto_Backend.Utils;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Resto_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserRepository userRepository;
        private readonly IAuthRepository _authRepository;
        private readonly TokenService _tokenService;
        public UserController(UserRepository userRepository, IAuthRepository authRepository, TokenService tokenService)
        {
            _authRepository = authRepository;
            _tokenService = tokenService;
            this.userRepository = userRepository;
        }


        #region Register User
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUser([FromForm] UserModel user)
        {
            try
            {
                if (user == null || user.ProfileImage == null)
                {
                    return BadRequest("User model or image is missing.");
                }

                bool isRegister = await userRepository.Register(user);
                if (isRegister)
                {
                    return Ok(new { Message = "User registered successfully!" });
                }

                return StatusCode(500, "An error occurred while registering the user.");
            }
            catch (Exception ex)
            {
                // Log the exception (for example, use Serilog, NLog, or Console.WriteLine)
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }


        #endregion
        [HttpGet("{id}")]
        #region Get User
        public async Task<IActionResult> GetUser(int userID)
        {
            var user = userRepository.SelectUserByPk(userID);
            Console.WriteLine(user);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
        #endregion

        [HttpPost]

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.UserName) || string.IsNullOrEmpty(model.Password))
                return BadRequest("Invalid request!");
            var user = userRepository.SelectUserByUserName(model.UserName);
            if (user == null)
            {
                return NotFound("User Not Found With Username");
            }

            try
            {
                string password = PasswordIncryptDecrypt.ConvertToDecrypt(user.Password);
                if (model.Password.Equals(password))
                {
                    string token = _tokenService.GenerateToken(model.UserName);

                    // Set token in HTTP-only cookie
                    HttpContext.Response.Cookies.Append("AuthToken", token, new CookieOptions
                    {
                        HttpOnly = true, // Prevents JavaScript from accessing the cookie
                        Secure = true,   // Ensures the cookie is sent only over HTTPS
                        SameSite = SameSiteMode.Strict, // Prevents cross-site request forgery (CSRF)
                        Expires = DateTime.UtcNow.AddHours(1) // Set cookie expiration
                    });

                    return Ok(new { Message = "Login successful!",Token = token  });
                }
                else
                {
                    return Unauthorized("Invalid Username or Password!");
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}
