using Flunt.Notifications;
using Flunt.Validations;

namespace api.Domain.DTOs
{
    public class FeedbackRequestDTO : Notifiable<Notification>
    {
        public string Description { get; set; }
        public string AuthorName { get; set; }
        public Guid AuthorId { get; set; }
        public Guid UserId { get; set; }

        public FeedbackRequestDTO(string description, string authorName, Guid authorId, Guid userId)
        {
            Description = description;
            AuthorName = authorName;
            AuthorId = authorId;
            UserId = userId;

            Validate();
        }

        private void Validate()
        {
            var contract = new Contract<FeedbackRequestDTO>()
                .IsNotNullOrEmpty(Description, "Description")
                .IsNotNullOrEmpty(AuthorName, "AuthorName")
                .IsNotNull(AuthorId, "AuthorId")
                .IsNotNull(UserId, "UserId");

            AddNotifications(contract);
        }
    }
}
