using Flunt.Notifications;
using Flunt.Validations;

namespace api.Domain.DTOs
{
    public class RecommendDTO : Notifiable<Notification>
    {
        public Guid UserId { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }

        public Guid AuthorId { get; set; }

        private void Validate()
        {
            var contract = new Contract<RecommendDTO>()
                .IsNotNullOrEmpty(Description, "Description")
                .IsNotNullOrEmpty(Name, "Name")
                .IsNotNull(UserId, "UserId")
                .IsNotNull(AuthorId, "AuthorId");



            AddNotifications(contract);
        }

        public RecommendDTO(string name, string description, Guid userId) 
        {
        
            this.Name = name;
            this.Description = description;
            this.UserId = userId;
            this.AuthorId = userId;

            Validate();
        }
    }
}
