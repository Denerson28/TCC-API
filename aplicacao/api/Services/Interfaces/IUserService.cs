using api.Domain.Classes;
using api.Domain.DTOs;
using Microsoft.EntityFrameworkCore;

namespace api.Services.Interfaces
{
    public interface IUserService
    {
        public List<UserRankingDTO> GetAll();
        public Task<UserSearchedDTO> Get(Guid id);
        public Task<UserResponseDTO> Create(UserRequestDTO userDTO);
        public Task<PublishResponseDTO> CreatePublish(Guid userId, PublishRequestDTO publish);
        public Task<UserUpdateDTO> Update(Guid Id,UserUpdateDTO userDTO);
        public Task<List<UserRankingDTO>> GetUsersRanking();
        public Task<List<PublishRequestDTO>> GetPublishesByUserId(Guid userId);
        public User GetUserByEmail(string email);
        public bool CheckPassword(Guid userId, string password);
        public Task<List<RecommendResponseDTO>> GetRecommendsByUserId(Guid userId);
        public Task<List<FeedbackResponseDTO>> GetFeedbacksByUserId(Guid userId);
        public Task<FeedbackResponseDTO> CreateFeedback(Guid userId, FeedbackRequestDTO feedback);
        public Task<RecommendResponseDTO> CreateRecommend(Guid userId, RecommendRequestDTO recommendation);

    }
}
