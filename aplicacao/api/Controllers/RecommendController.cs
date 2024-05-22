using api.Domain.DTOs;
using api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RockBank.Utils;

namespace api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class RecommendController
    {
        private readonly IRecommendService _recommendService;

        public RecommendController(IRecommendService recommendService)
        {
            _recommendService = recommendService;
        }

        [HttpPost]
        public async Task<IResult> Upload(RecommendDTO recommendDTO)
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
                var recommend = await _recommendService.Upload(recommendDTO);
                return Results.Created("recommend", recommend);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
        }
    }
}
