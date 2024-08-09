namespace api.Domain.Classes
{
    public class Publish : Entity
    {
        public string Image {  get; set; }
        public Guid UserId {  get; set; }
        public string Description {  get; set; }
        public string Title { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public Publish()
        {
            
        }

        public Publish(string image, string description, Guid userId)
        {
            this.Image = image;
            this.Description = description;
            this.UserId = userId;
        }
    }
}
