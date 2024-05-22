using api.Domain.Classes;
using api.Domain.DTOs;

namespace api.Services.Interfaces
{
    public interface IRecommendService
    {
        public Task<Recommend> Upload(RecommendDTO recommendation);
    }
}
