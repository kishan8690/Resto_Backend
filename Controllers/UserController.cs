using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Resto_Backend.Data;
using Resto_Backend.Middleware;
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
        [HttpPost]
        [Route("Register")]
        #region Register User
        public async Task<IActionResult> RegisterUser([FromBody] UserModel user)
        {
            if (user == null)
            {
                return BadRequest();
            }
            bool isRegister = await userRepository.Register(user);
            if (isRegister)
            {
                return Ok(new { Message = "User is Register SuccessFully !" });
            }
            return StatusCode(500, "An Error occurred while Registering");
        }
        #endregion
        [HttpGet("{id}")]
        #region Get User
        public async Task<IActionResult> GetUser(int userID)
        {
            var user = userRepository.SelectUserByPk(userID);
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
