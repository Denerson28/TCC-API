using Flunt.Notifications;
using Flunt.Validations;

namespace api.Domain.DTOs
{
    public class PdfFileDTO : Notifiable<Notification>
    {
        public string Name { get; set; }
        public string Content { get; set; }
        public string Description { get; set; }
        public Guid UserId {  get; set; }




        private void Validate()
        {
            var contract = new Contract<PdfFileDTO>()
                .IsNotNullOrEmpty(Name, "Name")
                .IsNotNullOrEmpty(Description, "Description")
                .IsNotNull(UserId, "UserId")
                .IsNotNullOrEmpty(Content, "Content");
                

            AddNotifications(contract);
        }
        public PdfFileDTO(string Name, string content, string description) 
        {
            this.Name = Name;
            this.Content = content;
            this.Description = description;

            Validate();
        }
    }
}
