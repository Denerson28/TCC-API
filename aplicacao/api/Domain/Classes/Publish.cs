namespace api.Domain.Classes
{
    public class Publish : Entity
    {
        public byte[] PdfContent {  get; set; }
        public Guid UserId {  get; set; }
        public string Description {  get; set; }
        public string Title { get; set; }
        public Publish()
        {
            
        }

        public Publish(byte[] content, string description, Guid userId)
        {
            this.PdfContent = content;
            this.Description = description;
            this.UserId = userId;
        }
    }
}
