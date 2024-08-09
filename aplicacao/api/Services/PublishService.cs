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

        public async Task Upload(Guid userId, PublishDTO publish)
        {
            // Verifica se foi enviado um arquivo
            if (string.IsNullOrEmpty(publish.Image))
            {
                throw new ArgumentException("Nenhum arquivo enviado.");
            }

            try
            {
            
                // Salva o arquivo no banco de dados
                var newPublishFile = new Publish
                {
                    Title = publish.Title,
                    Image = publish.Image,
                    UserId = userId, // Define o ID do usuário
                    Description = publish.Description
                };

                _context.Publishes.Add(newPublishFile);
                await _context.SaveChangesAsync();

                // Atualiza a lista de PDFs do usuário correspondente
                var user = await _context.Users.Include(u => u.Publishes).FirstOrDefaultAsync(u => u.Id == userId);
                if (user != null)
                {
                    user.Publishes.Add(newPublishFile);

                    // Incrementa 3 estrelas
                    user.Stars += 3;

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
