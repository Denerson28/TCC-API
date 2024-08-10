using api.Domain.Classes;
using Flunt.Notifications;
using Flunt.Validations;

namespace api.Domain.DTOs
{
    public class PublishResponseDTO : Notifiable<Notification>
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }




        private void Validate()
        {
            var contract = new Contract<PublishResponseDTO>()
                .IsNotNullOrEmpty(Description, "Description");


            AddNotifications(contract);
        }
        public PublishResponseDTO(Guid id, string title, string image, string description)
        {
            this.Id = id;
            this.Title = title;
            this.Image = image;
            this.Description = description;

            Validate();
        }

        public PublishResponseDTO(Publish publish)
        {
            this.Id = publish.Id;
            this.Title = publish.Title;
            this.Image = publish.Image;
            this.Description = publish.Description;
            this.CreatedAt = publish.CreatedAt;
        }

        public PublishResponseDTO() { }
    }
}

