using api.Domain.Classes;
using api.Domain.DTOs;
using Flunt.Notifications;
using Flunt.Validations;
using System.Diagnostics.Contracts;

namespace api.Domain.DTOs
{
    public class UserSearchedDTO : Notifiable<Notification>
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string TeamName { get; set; }
        public ICollection<Publish> Publishes { get; set; }
        public string Photo { get; set; }
        public int Stars { get; set; }

        private void Validate()
        {
            var contract = new Contract<UserSearchedDTO>()
                .IsNotNullOrEmpty(Name, "Name")
                .IsEmail(Email, "Email")
                .IsNotNullOrEmpty(Photo, "Photo")
                .IsNotNull(TeamName, "TeamId")
                .IsNotNull(Stars, "Stars")
                .IsNotNullOrEmpty(Role, "Role");

            AddNotifications(contract);

        }

        public UserSearchedDTO()
        {

        }

        public UserSearchedDTO(User user)
        {
            Name = user.Name;
            Photo = user.Photo;
            Email = user.Email;
            Role = user.Role;
            TeamName = user.TeamName;
            Publishes = user.Publishes;
            Stars = user.Stars;
            Validate();
        }
    }
}
