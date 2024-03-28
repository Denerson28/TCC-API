using api.Domain.Classes;
using api.Domain.DTOs;

namespace api.Services.Interfaces
{
    public interface IPdfFileService
    {
        public Task Upload(PdfFileDTO pdf);
    }
}
