﻿namespace api.Domain.Classes
{
    public class User : Entity
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserType { get; set; }
        public string Role { get; set; }
        public Guid TeamId { get; set; }
        public string Name { get; set; }

        public ICollection<Recommend> RecommendsReceived { get; set; }

        public List<Publish> Publishes { get; set; }
        public string Photo { get; internal set; }

        public User()
        {
            Publishes = new List<Publish>();
            RecommendsReceived = new List<Recommend>();
        }

        public User(string name, string photo, string email, string password,string userType, string role, Guid team)
        {
            
            this.Name = name;
            this.Email = email;
            this.Password = password;
            this.UserType = userType;
            this.Role = role;
            this.TeamId = team;
            this.Photo = photo;
            Publishes = new List<Publish>();
            RecommendsReceived = new List<Recommend>();
        }

    }
}
