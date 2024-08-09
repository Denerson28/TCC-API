using api.Domain.Classes;
using api.Domain.DTOs;
using api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RockBank.Utils;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        public LoginController(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }


        [HttpPost]
        public IResult Login([FromBody] LoginRequestDTO login)
        {
            if(login == null)
            {
                Results.BadRequest();
            }
            if (!login.IsValid)
            {
                return Results.ValidationProblem(login.Notifications.ConvertToProblemDetails());
            }

            User user = _userService.GetUserByEmail(login.Email);

            if (user == null)
            {
                Results.BadRequest();
            }
            if(!_userService.CheckPassword(user.Id, login.Password))
            {
                Results.BadRequest();
            }

            var key = Encoding.ASCII.GetBytes(_configuration["JwtBearerTokenSettings:SecretKey"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, login.Email),
                    new Claim("userType", user.UserType),
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Role, user.Role),
                    new Claim("userId", user.Id.ToString()),

                }),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Audience = _configuration["JwtBearerTokenSettings:Audience"],
                Issuer = _configuration["JwtBearerTokenSettings:Issuer"],
                Expires = DateTime.UtcNow.AddYears(1),
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Results.Ok(new
            {
                token = tokenHandler.WriteToken(token),
                user = new
                {
                    photo = user.Photo,
                    stars = user.Stars,
                    publishes = user.Publishes,
                }

            });

        }
    }
}
