using Flunt.Notifications;
using Flunt.Validations;

namespace api.Domain.DTOs
{
    public class PdfFileDTO : Notifiable<Notification>
    {
        public string Name { get; set; }
        public byte[] Content { get; set; }
        public string Description { get; set; }




        private void Validate()
        {
            var contract = new Contract<PdfFileDTO>()
                .IsNotNullOrEmpty(Name, "Name")
                .IsNotNullOrEmpty(Description, "Description")
                .IsNotNull(Content, "Content");

            AddNotifications(contract);
        }
        public PdfFileDTO(string Name, byte[] content, string description) 
        {
            this.Name = Name;
            this.Content = content;
            this.Description = description;

            Validate();
        }
    }
}
