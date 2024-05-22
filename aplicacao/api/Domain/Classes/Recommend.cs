﻿namespace api.Domain.Classes
{
    public class Recommend : Entity
    {
        public string Description { get; set; }

        public Recommend() { }

        public Guid UserId { get; set; }

        public Guid AuthorId { get; set; }
        public Recommend(string name, string description, Guid userId, Guid authorId) 
        { 
            this.Name = name;
            this.Description = description;
            this.UserId = userId;
            this.AuthorId = authorId;
        }

    }
}
