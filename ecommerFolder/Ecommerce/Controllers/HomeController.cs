using Ecommerce.DALs;
using Ecommerce.Models;
using Ecommerce.Webmodels;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Ecommerce.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly EcommerceContext _context;
        public static User user = new User();
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, EcommerceContext context, IConfiguration configuration)
        {
            _context = context;
            _logger = logger;
            _configuration = configuration;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return Ok();
        }

        [HttpGet("GetAllUser")]
        public async Task<IActionResult> GetAllUser()
        {
            var users = await _context.Users.ToListAsync();
            return Ok(users);
        }

        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
        {
            string passwordHash
                = BCrypt.Net.BCrypt.HashPassword(request.PasswordHash);
            _context.Users.Add(new User
            {
                Name = request.Name,
                Email = request.Email,
                Passwordhash = passwordHash,
                Role = request.Role,
            });

            await _context.SaveChangesAsync();
            return Ok(await GetAllUser());
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] Webmodels.LoginRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Name == request.Name);
            if (user == null)
            {
                return BadRequest("Not found");
            }
            else {
                string passwordHash
                = BCrypt.Net.BCrypt.HashPassword(request.Password);
                if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Passwordhash)) {
                    return BadRequest("Incorrect password");
                }
            }
            string token = CreateToken(user);
            return Ok(token);
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim> {                
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: creds
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
    }
}
