using api.Domain.Classes;
using api.Domain.DTOs;
using api.Infra.Data;
using api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace api.Services
{
    public class PublishService : IPublishService
    {
        private readonly ApplicationDbContext _context;

        public PublishService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Upload(PublishDTO pdf)
        {
            // Verifica se foi enviado um arquivo
            if (pdf == null || pdf.Content == null || pdf.Content.Length == 0)
            {
                throw new ArgumentException("Nenhum arquivo enviado.");
            }

            try
            {

                byte[] pdfBytes = Convert.FromBase64String(pdf.Content);
                // Salva o arquivo no banco de dados
                var newPdfFile = new Publish
                {
                    Title = pdf.Title,
                    PdfContent = pdfBytes,
                    UserId = pdf.UserId, // Define o ID do usuário
                    Description = pdf.Description
                };
                _context.Publishes.Add(newPdfFile);
                await _context.SaveChangesAsync();

                // Atualiza a lista de PDFs do usuário correspondente
                var user = await _context.Users.Include(u => u.Publishes).FirstOrDefaultAsync(u => u.Id == pdf.UserId);
                if (user != null)
                {
                    user.Publishes.Add(newPdfFile);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Erro ao enviar e salvar arquivo PDF: {ex.Message}");
            }
        }
    }
}
