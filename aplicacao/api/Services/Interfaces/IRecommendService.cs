using api.Domain.Classes;
using api.Domain.DTOs;

namespace api.Services.Interfaces
{
    public interface IRecommendService
    {
        public Task<RecommendDTO> Upload(RecommendDTO recommendation);
        Task<List<RecommendDTO>> GetRecommendations(Guid userId);
    }
}
