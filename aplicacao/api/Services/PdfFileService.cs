using api.Domain.Classes;
using api.Infra.Data;
using api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace api.Services
{
    public class PdfFileService : IPdfFileService
    {
        private readonly ApplicationDbContext _context;

        public PdfFileService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Upload(PdfFile pdf)
        {
            // Verifica se foi enviado um arquivo
            if (pdf == null || pdf.Content == null || pdf.Content.Length == 0)
            {
                throw new ArgumentException("Nenhum arquivo enviado.");
            }

            try
            {
                // Verifica se foi enviado um arquivo
                if (pdf == null || pdf.Content == null || pdf.Content.Length == 0)
                {
                    throw new ArgumentException("Nenhum arquivo enviado.");
                }

                // Salva o arquivo no banco de dados
                var newPdfFile = new PdfFile
                {
                    Name = pdf.Name,
                    Content = pdf.Content,
                    UserId = pdf.UserId // Define o ID do usuário
                };
                _context.PdfFiles.Add(newPdfFile);
                await _context.SaveChangesAsync();

                // Atualiza a lista de PDFs do usuário correspondente
                var user = await _context.Users.Include(u => u.PdfFiles).FirstOrDefaultAsync(u => u.Id == pdf.UserId);
                if (user != null)
                {
                    user.PdfFiles.Add(newPdfFile);
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
