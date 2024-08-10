using api.Domain.Classes;
using api.Domain.DTOs;
using api.Services;
using Flunt.Notifications;
using Flunt.Validations;
using System.Diagnostics.Contracts;

namespace api.Domain.DTOs
{
    public class UserRequestDTO : Notifiable<Notification>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserType { get; set; }
        public string Role { get; set; }
        public Guid TeamId { get; set; }
        public string Photo { get; set; }
        public string TeamName { get;  set; }

        private void Validate()
        {
            var contract = new Contract<UserRequestDTO>()
                .IsNotNullOrEmpty(Name, "Name")
                .IsEmail(Email, "Email")
                .IsNotNullOrEmpty(Password, "Password")
                .IsNotNullOrEmpty(UserType, "UserType")
                .IsNotNullOrEmpty(Role, "Role");
            
            AddNotifications(contract);

        }

        public UserRequestDTO()
        {

        }

        public UserRequestDTO(User user)
        {
            Id = user.Id;
            Name = user.Name;
            Photo = user.Photo;
            Email = user.Email;
            UserType = user.UserType;
            Role = user.Role;
            TeamId = user.TeamId;
            Validate();
        }
    }
}
