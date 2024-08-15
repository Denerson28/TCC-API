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

        [HttpGet("{userId}")]
        public async Task<IResult> Get(Guid userId)
        {
            try
            {
                var publish = await _publishService.Get(userId);

                return Results.Ok(publish);
            }
            catch (Exception ex)
            {
                return Results.StatusCode(500);
            }
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

        [HttpDelete("{userId}/{publishId}")]
        public async Task<IResult> Delete(Guid userId, Guid publishId)
        {
            try
            {
                await _publishService.Delete(userId, publishId);

                return Results.NoContent();
            }
            catch (Exception ex)
            {
                return Results.StatusCode(500);
            }
        }

        [HttpPut("{publishId}")]
        public async Task<IResult> Update(Guid publishId, [FromBody] PublishRequestDTO publish)
        {
            try
            {
                await _publishService.Update(publishId, publish);

                return Results.Ok();
            }
            catch (Exception ex)
            {
                return Results.StatusCode(500);
            }
        }
    }
}
