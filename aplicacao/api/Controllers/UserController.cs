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
        public async Task<IResult> CreateAsync(UserRequestDTO userDTO)
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
                UserResponseDTO user = await _userService.Create(userDTO);
                return Results.Created($"user/{user.Id}", user);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
        }

        [HttpDelete("{userId}")]
        public async Task<IResult> Delete(Guid userId)
        {
            try
            {
                await _userService.Delete(userId);
                return Results.NoContent();
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
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
                    return Results.NotFound();
                }

                return Results.Ok(userSearched);
            }
            catch (Exception ex)
            {
                return Results.StatusCode(500);
            }
        }

        [HttpPut("{userId}")]
        public async Task<IResult> UpdateUser(Guid userId, UserUpdateDTO userUpdateDTO)
        {
            try
            {
                UserUpdateDTO updatedUser = await _userService.Update(userId, userUpdateDTO);

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


        [HttpPost("{userId}/publish")]
        public async Task<IResult> CreatePublish(Guid userId, [FromBody] PublishRequestDTO publish)
        {
            try
            {
                var response = await _userService.CreatePublish(userId, publish);
                if (response == null)
                {
                    return Results.BadRequest();
                }
                return Results.Created();
            }
            catch (Exception ex)
            {
                return Results.StatusCode(500);
            }
        }
    }
}
