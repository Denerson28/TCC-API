using api.Domain.Classes;
using api.Domain.DTOs;
using api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
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

        public LoginController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpPost]
        public IResult Login([FromBody] LoginDTO login)
        {
            User user = _userService.GetUserByEmail(login.Email);

            if (user == null)
            {
                Results.BadRequest();
            }
            if(!_userService.CheckPassword(user.Id, login.Password))
            {
                Results.BadRequest();
            }

            var key = Encoding.ASCII.GetBytes("A@fderwfQQSDXCCer34asjsedlofASDS");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, login.Email)
                }),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Audience = "sqlserver",
                Issuer = "Issuer"
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Results.Ok(new
            {
                token = tokenHandler.WriteToken(token)
            });

        }
    }
}
