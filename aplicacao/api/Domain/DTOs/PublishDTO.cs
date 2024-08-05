using Flunt.Notifications;
using Flunt.Validations;

namespace api.Domain.DTOs
{
    public class PublishDTO : Notifiable<Notification>
    {
        public string Title { get; set; }
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
        public PublishDTO(string title, string content, string description, Guid userId) 
        {
            this.Title = title;
            this.Content = content;
            this.Description = description;
            this.UserId = userId;

            Validate();
        }
    }
}
