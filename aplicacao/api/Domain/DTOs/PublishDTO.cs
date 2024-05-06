using Flunt.Notifications;
using Flunt.Validations;

namespace api.Domain.DTOs
{
    public class PublishDTO : Notifiable<Notification>
    {
        public string Name { get; set; }
        public string Content { get; set; }
        public string Description { get; set; }
        public Guid UserId {  get; set; }




        private void Validate()
        {
            var contract = new Contract<PublishDTO>()
                .IsNotNullOrEmpty(Description, "Description")
                .IsNotNull(UserId, "UserId");
                

            AddNotifications(contract);
        }
        public PublishDTO(string Name, string content, string description, Guid userId) 
        {
            this.Name = Name;
            this.Content = content;
            this.Description = description;
            this.UserId = userId;

            Validate();
        }
    }
}
