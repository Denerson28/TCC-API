using api.Domain.Classes;
using Flunt.Notifications;
using Flunt.Validations;
using System.Data;

namespace api.Domain.DTOs
{
    public class TeamDTO : Notifiable<Notification>
    {
        public string Name { get; set; }

        private void Validate()
        {
            var contract = new Contract<TeamDTO>()
                .IsNotNullOrEmpty(Name, "Name");

            AddNotifications(contract);
        }

        public TeamDTO(string name)
        {
            Name = name;
            Validate();
        }
    }
}
