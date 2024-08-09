using api.Domain.Classes;
using Flunt.Notifications;

namespace api.Domain.DTOs
{
    public class UserRankingDTO : Notifiable<Notification>
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string ProfilePictureUrl { get; set; }
        public string TeamName { get; set; }
        public int Stars { get; set; }

        public UserRankingDTO(Guid userId, string name, string profilePictureUrl,string teamName, int stars)
        {
            UserId = userId;
            Name = name;
            ProfilePictureUrl = profilePictureUrl;
            Stars = stars;
        }

        public UserRankingDTO()
        {
        }

        public UserRankingDTO(User user)
        {
            UserId = user.Id;
            Name = user.Name;
            ProfilePictureUrl = user.Photo;
            TeamName = user.TeamName;
            Stars = user.Stars;
        }
    }
}
