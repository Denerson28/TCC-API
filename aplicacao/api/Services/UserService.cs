using api.Domain.Classes;
using api.Domain.DTOs;
using api.Infra.Data;
using api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace api.Services
{
    public class UserService : IUserService
    {

        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserResponseDTO> Create(UserRequestDTO userDTO)
        {
            using (var register = _context.Database.BeginTransaction())
            {

                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(userDTO.Password);
                try
                {
                    User user = new User(userDTO.Name, userDTO.Photo, userDTO.Email, hashedPassword,userDTO.UserType, userDTO.Role,userDTO.TeamName, userDTO.TeamId);

                    _context.Users.Add(user);

                    var team = await _context.Teams.Include(t => t.Users).FirstOrDefaultAsync(t => t.Id == userDTO.TeamId);
                    if (team != null)
                    {
                        team.Users.Add(user);
                    }
                    else
                    {
                        throw new Exception("Team not found");
                    }

                    await _context.SaveChangesAsync();
                    await register.CommitAsync();
                    UserResponseDTO userResponse = new UserResponseDTO(user);
                    return userResponse;
                }
                catch (Exception ex)
                {
                    await register.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task<UserSearchedDTO> Get(Guid id)
        {

            User user = _context.Users.Find(id);


            User userSearched =  await _context.Users
                .Include(r => r.RecommendsReceived)
                .Include(p => p.Publishes)
                .FirstOrDefaultAsync(u => u.Id == id);

            return new UserSearchedDTO(userSearched);
        }

        public List<UserRankingDTO> GetAll()
        {
            var users = _context.Users.ToList();
            var usersDto = new List<UserRankingDTO>();
            foreach (var user in users)
            {
                usersDto.Add(new UserRankingDTO(user));
            }
            return usersDto;
        }

        public async Task<UserUpdateDTO> Update(Guid userId, UserUpdateDTO userDTO)
        {
            var existingUser = await _context.Users.FindAsync(userId);

            if (existingUser == null)
            {
                throw new Exception("Usuário não encontrado");
            }

            existingUser.Name = userDTO.Name;
            existingUser.Photo = userDTO.Photo;

            await _context.SaveChangesAsync();

            return new UserUpdateDTO(existingUser);
        }

        public User GetUserByEmail(string email)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);

            if (user == null)
            {
                throw new Exception("Usuário não encontrado");
            }

            return user;
        }

        public bool CheckPassword(Guid userId, string password)
        {
            var user = _context.Users.Find(userId);

            if (user == null)
            {
                throw new Exception("Usuário não encontrado");
            }

            return BCrypt.Net.BCrypt.Verify(password, user.Password);
        }

        public async Task<List<UserRankingDTO>> GetUsersRanking()
        {
            var usersRanking = await _context.Users
            .OrderByDescending(u => u.Stars)
            .Select(u => new UserRankingDTO(
            u.Id,
            u.Name,
            u.Photo,
            u.TeamName,
            u.Stars))
            .ToListAsync();

            return usersRanking;
        }

        public async Task<List<PublishRequestDTO>> GetPublishesByUserId(Guid userId)
        {
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                throw new Exception("Usuário não encontrado");
            }

            var publishesEntity = await _context.Publishes.Where(p => p.UserId == userId).ToListAsync();

            List<PublishRequestDTO> publishes = new List<PublishRequestDTO>();

            foreach (var publish in publishesEntity)
            {
                publishes.Add(new PublishRequestDTO(publish));
            }

            return publishes;
        }

        public async Task<List<RecommendResponseDTO>> GetRecommendsByUserId(Guid userId)
        {
            var recommendations = await _context.Recommendations
                .Where(r => r.UserId == userId)
                .Select(r => new RecommendResponseDTO(
                    r.UserId,
                    r.AuthorId,
                    _context.Users.Where(u => u.Id == r.AuthorId).Select(u => u.Name).FirstOrDefault(),
                    _context.Users.Where(u => u.Id == r.AuthorId).Select(u => u.Photo).FirstOrDefault(),
                    r.Description))
                .ToListAsync();

            return recommendations;
        }

        public async Task<List<FeedbackResponseDTO>> GetFeedbacksByUserId(Guid userId)
        {
            var feedbacks = await _context.Feedbacks
                .Where(r => r.UserId == userId)
                .Select(r => new FeedbackResponseDTO(
                    r.UserId,
                    r.AuthorId,
                    _context.Users.Where(u => u.Id == r.AuthorId).Select(u => u.Name).FirstOrDefault(),
                    _context.Users.Where(u => u.Id == r.AuthorId).Select(u => u.Photo).FirstOrDefault(),
                    r.Description,
                    r.CreatedAt))
                .ToListAsync();

            return feedbacks;
        }

        public async Task<FeedbackResponseDTO> CreateFeedback(Guid userId, FeedbackRequestDTO feedback)
        {
            if (feedback == null)
            {
                throw new ArgumentNullException(nameof(feedback), "feedbackDTO cannot be null");
            }

            try
            {
                var newFeedback = new Feedback
                {
                    Description = feedback.Description,
                    UserId = userId,
                    AuthorId = feedback.AuthorId
                };

                _context.Feedbacks.Add(newFeedback);
                await _context.SaveChangesAsync();

                var user = await _context.Users.Include(u => u.FeedbacksReceived)
                                               .FirstOrDefaultAsync(u => u.Id == userId);
                if (user != null)
                {
                    user.FeedbacksReceived.Add(newFeedback);
                    await _context.SaveChangesAsync();
                }

                var author = await _context.Users.FindAsync(feedback.AuthorId);
                if (author == null)
                {
                    throw new ApplicationException("Author not found");
                }

                var responseDTO = new FeedbackResponseDTO(
                    userId,
                    newFeedback.AuthorId,
                    author.Name,
                    author.Photo,
                    newFeedback.Description,
                    newFeedback.CreatedAt
                );

                return responseDTO;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error while sending and saving recommendation: {ex.Message}", ex);
            }
        }

        public async Task<RecommendResponseDTO> CreateRecommend(Guid userId, RecommendRequestDTO recommendation)
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
                    UserId = userId,
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

                var responseDTO = new RecommendResponseDTO(
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

        public async Task<PublishResponseDTO> CreatePublish(Guid userId, PublishRequestDTO publish)
        {
            if (string.IsNullOrEmpty(publish.Image))
            {
                throw new ArgumentException("Nenhum arquivo enviado.");
            }

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
                await _context.SaveChangesAsync();

                var user = await _context.Users.Include(u => u.Publishes).FirstOrDefaultAsync(u => u.Id == userId);
                if (user != null)
                {
                    user.Publishes.Add(newPublishFile);

                    user.Stars += 3;

                    await _context.SaveChangesAsync();

                    return new PublishResponseDTO(newPublishFile);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Erro ao enviar e salvar publicacao: {ex.Message}");
            }
        }
    }
}
