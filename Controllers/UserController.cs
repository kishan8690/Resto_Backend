using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Resto_Backend.Data;
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
            var user  = userRepository.SelectUserByUserName(model.UserName);
            if (user == null)
            {
                return NotFound("User Not Found With Username");
            }
            string password = PasswordIncryptDecrypt.ConvertToDecrypt(user.Password);
            //Console.WriteLine("password-" + password);
            if (model.Password.Equals(password))
            {
                string token = _tokenService.GenerateToken(model.UserName);
                return Ok(new { Token = token });
            }
            else
            {
                return Unauthorized("Invalid Email or Password!");
            }    
        }

    }
}
