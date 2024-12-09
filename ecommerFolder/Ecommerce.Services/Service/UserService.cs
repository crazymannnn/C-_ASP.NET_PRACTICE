using Ecommerce.Models;
using Ecommerce.Repositories.IRepositories;
using Ecommerce.Services.IService;
using Ecommerce.Webmodels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Services.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public UserService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public List<User> GetAll() { 
            return _userRepository.GetAll();
        }

        public void CreateUser(CreateUserRequest request)
        {
            string passwordHash
                = BCrypt.Net.BCrypt.HashPassword(request.PasswordHash);
            _userRepository.Create(new User
            {
                Name = request.Name,
                Email = request.Email,
                Passwordhash = passwordHash,
                Role = request.Role,
            });
        }

        public string Login(LoginRequest request) {
            var people = GetAll();
            //var user = await _context.Users.FirstOrDefaultAsync(x => x.Name == request.Name);
            var user = people.FirstOrDefault(x => x.Name == request.Name);
            if (user == null)
            {
                return null;
            }
            else
            {
                string passwordHash
                = BCrypt.Net.BCrypt.HashPassword(request.Password);
                if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Passwordhash))
                {
                    return null;
                }
            }
            string token = CreateToken(user);
            return token;
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
