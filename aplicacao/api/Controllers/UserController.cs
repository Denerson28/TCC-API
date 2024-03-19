using api.Domain.Classes;
using api.Domain.DTOs;
using api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using RockBank.Utils;

namespace api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController
    {

        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpPost]
        public async Task<IResult> CreateAsync(UserDTO userDTO)
        {
            if(userDTO == null)
            {
                return Results.BadRequest();
            }

            if (!userDTO.IsValid)
            {
                return Results.ValidationProblem(userDTO.Notifications.ConvertToProblemDetails());
            }

            try
            {
                User user = await _userService.Create(userDTO);
                return Results.Created($"user/{user.Id}", user); // Retorna um resultado Ok com o usuário criado
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message); // Retorna um resultado de erro com a mensagem de exceção
            }


        }
    }
}
