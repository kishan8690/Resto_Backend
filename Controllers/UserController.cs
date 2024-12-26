using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Resto_Backend.Data;
using Resto_Backend.Model;

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

        public async Task<IActionResult> RegisterUser([FromBody] UserModel user)
        {
            if (user == null)
            {
                return BadRequest();
            }
            bool isRegister=await userRepository.Register(user);
            if (isRegister)
            {
                return Ok(new { Message = "User is Register SuccessFully !" });
            }
            return StatusCode(500, "An Error occurred while Registering");
        }
       
        [HttpGet("{id}")]
      
        public async Task<IActionResult> GetUser(int userID)
        {
            var user = userRepository.SelectUserByPk(userID);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
        
    }
}
