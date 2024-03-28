using api.Domain.Classes;
using api.Domain.DTOs;
using api.Infra.Data;
using api.Services.Interfaces;
using Azure.Core;
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

        public async Task Upload(PdfFileDTO pdf)
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

                byte[] pdfBytes = Convert.FromBase64String(pdf.Content);
                // Salva o arquivo no banco de dados
                var newPdfFile = new PdfFile
                {
                    Name = pdf.Name,
                    Content = pdfBytes,
                    UserId = pdf.UserId, // Define o ID do usuário
                    Description = pdf.Description
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
