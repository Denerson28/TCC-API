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

        public async Task<PublishResponseDTO> Upload(Guid userId, PublishRequestDTO publish)
        {
            try
            {
            
                var newPublishFile = new Publish
                {
                    Title = publish.Title,
                    Image = publish.Image,
                    UserId = userId,
                    Description = publish.Description
                };

                _context.Publishes.Add(newPublishFile);

                var user = await _context.Users.Include(u => u.Publishes).FirstOrDefaultAsync(u => u.Id == userId);
                if (user != null)
                {
                    user.Publishes.Add(newPublishFile);

                    user.Stars += 3;

                    await _context.SaveChangesAsync();

                    return new PublishResponseDTO(newPublishFile);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Erro ao enviar e salvar publicacao: {ex.Message}");
            }

            return null;
        }
    }
}
