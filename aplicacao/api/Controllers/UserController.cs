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

        [HttpGet("{userId}")]
        public async Task<IResult> Get(Guid userId)
        {

            try
            {
                UserSearchedDTO userSearched = await _userService.Get(userId);

                if (userSearched == null)
                {
                    return Results.NotFound(); // Retorna NotFound se o usuário não for encontrado
                }

                return Results.Ok(userSearched); // Retorna o usuário encontrado
            }
            catch (Exception ex)
            {
                return Results.StatusCode(500); // Retorna um erro interno do servidor se ocorrer uma exceção
            }
        }

        [HttpPut("{Id}")]
        public async Task<IResult> UpdateUser(Guid Id, UserUpdateDTO userUpdateDTO)
        {
            try
            {
                UserUpdateDTO updatedUser = await _userService.Update(Id, userUpdateDTO);

                return Results.Ok(updatedUser);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
        }

        [HttpGet("ranking")]
        public async Task<IResult> GetUsersRanking()
        {
            var ranking = await _userService.GetUsersRanking();
            return Results.Ok(ranking);
        }

        [HttpGet("{userId}/publishes")]
        public async Task<IResult> GetPublishesByUserId(Guid userId)
        {
            var publishes = await _userService.GetPublishesByUserId(userId);
            return Results.Ok(publishes);
        }

        [HttpGet("{userId}/recommends")]
        public async Task<IResult> GetRecommendsByUserId(Guid userId)
        {
            var recommends = await _userService.GetRecommendsByUserId(userId);
            return Results.Ok(recommends);
        }

        [HttpPost("{userId}/recommend")]
        public async Task<IResult> Create(Guid userId, RecommendRequestDTO recommendDTO)
        {
            if (recommendDTO == null)
            {
                return Results.BadRequest();
            }

            if (!recommendDTO.IsValid)
            {
                return Results.ValidationProblem(recommendDTO.Notifications.ConvertToProblemDetails());
            }

            try
            {
                var recommend = await _userService.CreateRecommend(userId, recommendDTO);
                return Results.Created("recommend", recommend);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
        }

        [HttpGet("{userId}/feedbacks")]
        public async Task<IResult> GetFeedbacksByUserId(Guid userId)
        {
            var feedbacks = await _userService.GetFeedbacksByUserId(userId);
            return Results.Ok(feedbacks);
        }

        [HttpGet]
        public async Task<IResult> GetAll()
        {
            List<UserRankingDTO> users = _userService.GetAll();

            return Results.Ok(users);
        }

        [HttpPost("{userId}/feedback")]
        public async Task<IResult> Create(Guid userId, FeedbackRequestDTO feedbackRequestDTO)
        {
            if (feedbackRequestDTO == null)
            {
                return Results.BadRequest();
            }

            if (!feedbackRequestDTO.IsValid)
            {
                return Results.ValidationProblem(feedbackRequestDTO.Notifications.ConvertToProblemDetails());
            }

            try
            {
                var feedback = await _userService.CreateFeedback(userId, feedbackRequestDTO);
                return Results.Created("Feedback", feedback);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
        }
    }
}
