using api.Domain.Classes;
using api.Domain.DTOs;
using api.Services;
using api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RockBank.Utils;

namespace api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UserController
    {

        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize(Policy = "AdminPolicy")]
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

        [HttpGet]
        public  IResult Get(Guid id)
        {

            try
            {
                User user = _userService.Get(id);

                if (user == null)
                {
                    return Results.NotFound(); // Retorna NotFound se o usuário não for encontrado
                }

                return Results.Ok(user); // Retorna o usuário encontrado
            }
            catch (Exception ex)
            {
                return Results.StatusCode(500); // Retorna um erro interno do servidor se ocorrer uma exceção
            }
        }

        [HttpPut("{Id}")]
        public async Task<IResult> UpdateUser(Guid Id, UserDTO userDTO)
        {
            try
            {
                User updatedUser = await _userService.Update(Id, userDTO);

                return Results.Ok(updatedUser);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
        }
    }
}
