using Flunt.Notifications;
using Flunt.Validations;
using System.Diagnostics.Contracts;

namespace api.Domain.DTOs
{
    public class LoginDTO : Notifiable<Notification>
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public void Validate()
        {
            var contract = new Contract<LoginDTO>()
                .IsEmail(Email, "Email")
                .IsNotNullOrEmpty(Password, "Password");
                

            AddNotifications(contract);
        }


        public LoginDTO(string email, string password)
        {
            this.Email = email;
            this.Password = password;

            Validate();
        }
    }
}
