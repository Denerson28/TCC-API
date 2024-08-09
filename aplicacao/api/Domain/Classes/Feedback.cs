namespace api.Domain.Classes
{
    public class Feedback : Entity
    {
        public string Description { get; set; }
        public Guid UserId { get; set; }

        public Guid AuthorId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public Feedback() { }

        public Feedback(string description, Guid userId, Guid authorId)
        {
            this.Description = description;
            this.UserId = userId;
            this.AuthorId = authorId;
        }
    }
}
