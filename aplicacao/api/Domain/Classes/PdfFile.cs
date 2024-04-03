namespace api.Domain.Classes
{
    public class PdfFile : Entity
    {
        public byte[] Content {  get; set; }
        public Guid UserId {  get; set; }
        public string Description {  get; set; }


        public PdfFile()
        {
            
        }

        public PdfFile(string name, byte[] content, string description, Guid userId)
        {
            this.Name = name;
            this.Content = content;
            this.Description = description;
            this.UserId = userId;
        }
    }
}
