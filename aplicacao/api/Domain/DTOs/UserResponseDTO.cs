using api.Domain.Classes;
using Flunt.Notifications;
using Flunt.Validations;

namespace api.Domain.DTOs
{
    public class UserResponseDTO : Notifiable<Notification>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserType { get; set; }
        public string Role { get; set; }
        public Guid TeamId { get; set; }
        public int Stars { get; set; }
        public ICollection<Publish> Publishes { get; set; }
        public ICollection<Recommend> RecommendsReceived { get; set; }
        public ICollection<Feedback> FeedbacksReceived { get; set; }
        public string Photo { get; set; }
        public string TeamName { get; set; }

        private void Validate()
        {
            var contract = new Contract<UserResponseDTO>()
                .IsNotNullOrEmpty(Name, "Name")
                .IsEmail(Email, "Email")
                .IsNotNullOrEmpty(Password, "Password")
                .IsNotNullOrEmpty(UserType, "UserType")
                .IsNotNullOrEmpty(Role, "Role");

            AddNotifications(contract);

        }

        public UserResponseDTO()
        {

        }

        public UserResponseDTO(User user)
        {
            Id = user.Id;
            Name = user.Name;
            Photo = user.Photo;
            Email = user.Email;
            UserType = user.UserType;
            Role = user.Role;
            TeamId = user.TeamId;
            Publishes = user.Publishes;
            RecommendsReceived = user.RecommendsReceived;
            FeedbacksReceived = user.FeedbacksReceived;
            Validate();
        }
    }
}
