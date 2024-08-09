using api.Domain.Classes;
using Flunt.Notifications;
using Flunt.Validations;

namespace api.Domain.DTOs
{
    public class FeedbackResponseDTO : Notifiable<Notification>
    {
        public Guid UserId { get; set; }
        public Guid AuthorId { get; set; }
        public string AuthorName { get; set; }
        public string AuthorProfilePictureUrl { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }

        public FeedbackResponseDTO(Guid userId, Guid authorId, string authorName, string authorProfilePictureUrl, string description, DateTime createdAt)
        {
            UserId = userId;
            AuthorId = authorId;
            AuthorName = authorName;
            AuthorProfilePictureUrl = authorProfilePictureUrl;
            CreatedAt = createdAt;
            Description = description;

            Validate();
        }

        public FeedbackResponseDTO()
        {
        }

        private void Validate()
        {
            var contract = new Contract<FeedbackResponseDTO>()
                .IsNotNullOrEmpty(Description, "Description")
                .IsNotNullOrEmpty(AuthorProfilePictureUrl, "AuthorProfilePictureUrl")
                .IsNotNullOrEmpty(AuthorName, "AuthorName")
                .IsNotNull(UserId, "UserId")
                .IsNotNull(AuthorId, "AuthorId")
                .IsNotNull(CreatedAt, "CreatedAt");

            AddNotifications(contract);
        }
    }
}
