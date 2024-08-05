﻿using Flunt.Notifications;
using Flunt.Validations;

namespace api.Domain.DTOs
{
    public class RecommendDTO : Notifiable<Notification>
    {
    public Guid UserId { get; set; }
    public Guid AuthorId { get; set; }
    public string AuthorName { get; set; }
    public string AuthorProfilePictureUrl { get; set; }
    public string Description { get; set; }

    public RecommendDTO(Guid userId, Guid authorId, string authorName, string authorProfilePictureUrl, string description)
    {
        UserId = userId;
        AuthorId = authorId;
        AuthorName = authorName;
        AuthorProfilePictureUrl = authorProfilePictureUrl;
        Description = description;

        Validate();
    }

        private void Validate()
        {
            var contract = new Contract<RecommendDTO>()
                .IsNotNullOrEmpty(Description, "Description")
                .IsNotNullOrEmpty(AuthorProfilePictureUrl, "AuthorProfilePictureUrl")
                .IsNotNullOrEmpty(AuthorName, "AuthorName")
                .IsNotNull(UserId, "UserId")
                .IsNotNull(AuthorId, "AuthorId");

            AddNotifications(contract);
        }
    }
}
