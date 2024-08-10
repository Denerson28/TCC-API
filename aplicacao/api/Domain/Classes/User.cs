namespace api.Domain.Classes
{
    public class User : Entity
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserType { get; set; }
        public string Role { get; set; }
        public Guid TeamId { get; set; }
        public string TeamName { get; set; }
        public string Name { get; set; }
        public int Stars { get; set; } = 0;

        public ICollection<Recommend> RecommendsReceived { get; set; }

        public ICollection<Feedback> FeedbacksReceived { get; set; }

        public ICollection<Publish> Publishes { get; set; }
        public string Photo { get;  set; }

        public User()
        {
            Publishes = new List<Publish>();
            RecommendsReceived = new List<Recommend>();
            FeedbacksReceived = new List<Feedback>();
        }

        public User(string name, string photo, string email, string password,string userType, string role, string teamName, Guid team)
        {
            
            this.Name = name;
            this.Email = email;
            this.Password = password;
            this.UserType = userType;
            this.Role = role;
            this.TeamName = teamName;
            this.TeamId = team;
            this.Photo = photo;
            Publishes = new List<Publish>();
            RecommendsReceived = new List<Recommend>();
            FeedbacksReceived = new List<Feedback>();
        }

    }
}
