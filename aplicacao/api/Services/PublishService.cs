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

        public async Task<PublishResponseDTO> Get(Guid publishId)
        {
            try
            {
                var publish = await _context.Publishes.FirstOrDefaultAsync(u => u.Id == publishId);
                if (publish != null)
                {
                    return new PublishResponseDTO(publish);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Erro ao buscar publicacoes: {ex.Message}");
            }

            return null;
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

        public async Task Delete(Guid userId, Guid publishId)
        {
            try
            {
                var user = await _context.Users.Include(u => u.Publishes).FirstOrDefaultAsync(u => u.Id == userId);
                if (user != null)
                {
                    var publish = user.Publishes.FirstOrDefault(p => p.Id == publishId);
                    if (publish != null)
                    {
                        user.Publishes.Remove(publish);
                        _context.Publishes.Remove(publish);

                        await _context.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Erro ao deletar publicacao: {ex.Message}");
            }
        }

        public async Task Update(Guid publishId, PublishRequestDTO publish)
        {
            try
            {
                var publishFile = await _context.Publishes.FirstOrDefaultAsync(p => p.Id == publishId);
                if (publishFile != null)
                {
                    publishFile.Title = publish.Title;
                    publishFile.Image = publish.Image;
                    publishFile.Description = publish.Description;

                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Erro ao atualizar publicacao: {ex.Message}");
            }
        }
    }
}
