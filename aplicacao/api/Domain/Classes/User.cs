﻿namespace api.Domain.Classes
{
    public class User : Entity
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public Guid TeamId { get; set; }
        public List<PdfFile> PdfFiles { get; set; }

        public User()
        {
            PdfFiles = new List<PdfFile>();
        }

        public User(string name, string email, string password, string role, Guid team)
        {
            
            this.Name = name;
            this.Email = email;
            this.Password = password;
            this.Role = role;
            this.TeamId = team;
            PdfFiles = new List<PdfFile>();
        }

    }
}
