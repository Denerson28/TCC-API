using api.Domain.Classes;
using api.Domain.DTOs;
using api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PdfFileController
    {
        private readonly IPdfFileService _pdfFileService;

        public PdfFileController(IPdfFileService pdfFileService) 
        {
            _pdfFileService = pdfFileService;
        }


        [HttpPost]
        public async Task<IResult> UploadFile([FromBody] PdfFileDTO pdf)
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
