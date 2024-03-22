using api.Domain.Classes;

namespace api.Services.Interfaces
{
    public interface IPdfFileService
    {
        public Task Upload(PdfFile pdf);
    }
}
