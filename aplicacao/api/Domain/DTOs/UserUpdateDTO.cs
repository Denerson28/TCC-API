using api.Domain.Classes;
using Flunt.Notifications;
using Flunt.Validations;

namespace api.Domain.DTOs
{
    public class UserUpdateDTO : Notifiable<Notification>
    {
        public string Name { get; set; }
        public string Photo { get; set; }

        private void Validate()
        {
            var contract = new Contract<UserUpdateDTO>()
                .IsNotNullOrEmpty(Name, "Name")
                .IsNotNullOrEmpty(Photo, "Photo");

            AddNotifications(contract);
        }

        public UserUpdateDTO()
        {

        }

        public UserUpdateDTO(User user)
        {
            Name = user.Name;
            Photo = user.Photo;
            Validate();
        }
    }
}
