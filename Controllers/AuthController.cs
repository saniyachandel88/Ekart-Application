using Ekart_Application.IServices;
using Ekart_web_Application.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ekart_Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("Login")]
        public ActionResult<string> Login([FromBody] LoginDto obj)
        {
            if (obj == null || string.IsNullOrEmpty(obj.UserName) || string.IsNullOrEmpty(obj.Password))
            {
                return BadRequest("Username and password must be provided.");
            }

            var token = _authService.Login(obj);
            return Ok(token);


        }
        [HttpPost("AddUser")]
        public ActionResult<UserWithToken> AddUser([FromBody] CreateUserDto user)
        {
            if (user == null || string.IsNullOrEmpty(user.UserName) || string.IsNullOrEmpty(user.Password))
            {
                return BadRequest("Invalid user data.");
            }


            // Call the service to add the user and generate the token
            var userWithToken = _authService.AddUser(user);
            return Ok(userWithToken);

        }
    }
}
