using api.Domain.Classes;
using api.Domain.DTOs;
using api.Infra.Data;
using api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace api.Services
{
    public class RecommendService : IRecommendService
    {
        private readonly ApplicationDbContext _context;

        public RecommendService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Recommend> Upload(RecommendDTO recommendation)
        {
            if(recommendation == null) 
            {
                throw new ArgumentNullException("RecommendDTO cannot be null");
            }

            try
            {
                var newRecommendation = new Recommend
                {
                    Name = recommendation.Name,
                    UserId = recommendation.UserId,
                    Description = recommendation.Description,
                    AuthorId = recommendation.AuthorId
                };

                _context.Recommendations.Add(newRecommendation);
                await _context.SaveChangesAsync();

                var user = await _context.Users.Include(u => u.RecommendsReceived).FirstOrDefaultAsync(u => u.Id == recommendation.UserId);
                if (user != null)
                {
                    user.RecommendsReceived.Add(newRecommendation);
                    await _context.SaveChangesAsync();
                }

                return newRecommendation;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error while sending and saving recommendation: {ex.Message}");
            }


        }
    }
}
