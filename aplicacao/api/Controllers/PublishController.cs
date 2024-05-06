using api.Domain.Classes;
using api.Domain.DTOs;
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
        private readonly IPublishService _pdfFileService;

        public PublishController(IPublishService pdfFileService) 
        {
            _pdfFileService = pdfFileService;
        }


        [HttpPost]
        public async Task<IResult> UploadFile([FromBody] PublishDTO pdf)
        {
            try
            {
                await _pdfFileService.Upload(pdf);

                return Results.Ok("Arquivo PDF enviado e salvo com sucesso no banco de dados.");
            }
            catch (Exception ex)
            {
                return Results.StatusCode(500);
            }
        }
    }
}
