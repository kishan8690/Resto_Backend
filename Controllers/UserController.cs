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

        public UserController(UserRepository userRepository)
        {
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
        [Route("Login")]
        #region Login
        public IActionResult Login([FromBody] LoginModel loginModel)
        {
            Console.WriteLine("User name=" + loginModel.UserName);
            Console.WriteLine("Password=" + loginModel.Password);
            var user = userRepository.SelectUserByUserName(loginModel.UserName);
            if (user == null)
            {
                return NotFound();
            }
            Console.WriteLine("after User name=" + user.UserName);
            Console.WriteLine("after Password=" + user.Password);
            //bool isValidUser = PasswordIncryptDecrypt.ConvertToDecrypt(loginModel.Password, user.Password);
            //if (!isValidUser)
            //{
            //    return NotFound("Incorrect Password or User Name");
            //}


            return Ok(loginModel);
            //if(user != null)
            //{
            //    var chaims = new[]
            //    {
            //        new Claim(JwtRegisteredClaimNames.Sub,userRepository._configuration["Jwt:Subject"]),
            //        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            //        new Claim("UserID",user.UserID.ToString()),
            //        new Claim("UserName",user.UserName.ToString()),
            //    };
            //    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(userRepository._configuration["Jwt:Key"]));
            //    var signIn = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);
            //    var token = new JwtSecurityToken(userRepository._configuration["Jwt:Issuer"]
            //        , userRepository._configuration["Jwt:Audience"]
            //        ,chaims,expires:DateTime.UtcNow.AddMinutes(60),
            //        signingCredentials:signIn
            //        );
            //    string tokenValue  = new JwtSecurityTokenHandler().WriteToken(token);
            //    return Ok(new {Token = tokenValue,User = user});
            //}
            //return NoContent();
        }
        #endregion
        
    }
}
