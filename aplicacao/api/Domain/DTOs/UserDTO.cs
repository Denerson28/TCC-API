using api.Domain.Classes;
using api.Domain.DTOs;
using Flunt.Notifications;
using Flunt.Validations;
using System.Diagnostics.Contracts;

namespace api.Domain.DTOs
{
    public class UserDTO : Notifiable<Notification>
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserType { get; set; }
        public string Role { get; set; }
        public Guid TeamId { get; set; }
        public List<Publish> Publishes { get; set; }

        private void Validate()
        {
            var contract = new Contract<UserDTO>()
                .IsNotNullOrEmpty(Name, "Name")
                .IsNotNullOrEmpty(Email, "Email")
                .IsNotNullOrEmpty(Password, "Password")
                .IsNotNullOrEmpty(UserType, "UserType")
                .IsNotNullOrEmpty(Role, "Role");
            
            AddNotifications(contract);

        }

        public UserDTO(string name, string email, string password, string userTpe, string role, Guid teamId)
        {
            Name = name;
            Email = email;
            Password = password;
            UserType = userTpe;
            Role = role;
            TeamId = teamId;
            Publishes = new List<Publish>();
            Validate();
        }
    }
}
