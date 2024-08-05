using api.Domain.Classes;
using api.Domain.DTOs;
using api.Infra.Data;
using api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Services
{
    public class RecommendService : IRecommendService
    {
        private readonly ApplicationDbContext _context;

        public RecommendService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<RecommendDTO> Upload(RecommendDTO recommendation)
        {
            if (recommendation == null)
            {
                throw new ArgumentNullException(nameof(recommendation), "RecommendDTO cannot be null");
            }

            try
            {
                var newRecommendation = new Recommend
                {
                    Description = recommendation.Description,
                    UserId = recommendation.UserId,
                    AuthorId = recommendation.AuthorId
                };

                _context.Recommendations.Add(newRecommendation);
                await _context.SaveChangesAsync();

                var user = await _context.Users.Include(u => u.RecommendsReceived)
                                               .FirstOrDefaultAsync(u => u.Id == recommendation.UserId);
                if (user != null)
                {
                    user.RecommendsReceived.Add(newRecommendation);
                    await _context.SaveChangesAsync();
                }

                var author = await _context.Users.FindAsync(recommendation.AuthorId);
                if (author == null)
                {
                    throw new ApplicationException("Author not found");
                }

                var responseDTO = new RecommendDTO(
                    newRecommendation.UserId,
                    newRecommendation.AuthorId,
                    author.Name,
                    author.Photo,
                    newRecommendation.Description
                );

                return responseDTO;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error while sending and saving recommendation: {ex.Message}", ex);
            }
        }

        public async Task<List<RecommendDTO>> GetRecommendations(Guid userId)
        {
            var recommendations = await _context.Recommendations
                .Where(r => r.UserId == userId)
                .Select(r => new RecommendDTO(
                    r.UserId,
                    r.AuthorId,
                    _context.Users.Where(u => u.Id == r.AuthorId).Select(u => u.Name).FirstOrDefault(),
                    _context.Users.Where(u => u.Id == r.AuthorId).Select(u => u.Photo).FirstOrDefault(),
                    r.Description))
                .ToListAsync();

            return recommendations;
        }
    }
}
