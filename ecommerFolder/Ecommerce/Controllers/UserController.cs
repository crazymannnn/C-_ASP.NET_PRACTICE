using Ecommerce.Services.IService;
using Ecommerce.Webmodels;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService) { 
            _userService = userService;
        }

        [HttpGet("GetAllUser")]
        public IActionResult Get()
        {
            var users = _userService.GetAll();    
            return Ok(users);
        }

        [HttpPost("CreateUser")]
        public IActionResult Create(CreateUserRequest request)
        {
            _userService.CreateUser(request);
            return Ok();
        }

        [HttpPost("Login")]
        public IActionResult Login(LoginRequest request) { 
            var token = _userService.Login(request);
            if (token == null) return BadRequest("Not found or Incorrect password");
            return Ok(token);
        }
    }
}
