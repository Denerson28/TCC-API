using api.Domain.Classes;
using api.Domain.DTOs;
using api.Services;
using api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class PublishController
    {
        private readonly IPublishService _publishService;

        public PublishController(IPublishService publishService) 
        {
            _publishService = publishService;
        }


        [HttpPost("{userId}")]
        public async Task<IResult> Create(Guid userId, [FromBody] PublishRequestDTO publish)
        {
            try
            {
                await _publishService.Upload(userId, publish);

                return Results.Created();
            }
            catch (Exception ex)
            {
                return Results.StatusCode(500);
            }
        }
    }
}
