namespace api.Domain.Classes
{
    public class Recommend : Entity
    {
        public string Description { get; set; }
        public Guid UserId { get; set; }

        public Guid AuthorId { get; set; }
        public Recommend() { }
        public Recommend(string description, Guid userId, Guid authorId) 
        { 
            this.Description = description;
            this.UserId = userId;
            this.AuthorId = authorId;
        }

    }
}
