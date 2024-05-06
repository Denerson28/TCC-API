namespace api.Domain.Classes
{
    public class User : Entity
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserType { get; set; }
        public string Role { get; set; }
        public Guid TeamId { get; set; }

        public List<Publish> Publishes { get; set; }

        public User()
        {
            Publishes = new List<Publish>();
        }

        public User(string name, string email, string password,string userType, string role, Guid team)
        {
            
            this.Name = name;
            this.Email = email;
            this.Password = password;
            this.UserType = userType;
            this.Role = role;
            this.TeamId = team;
            Publishes = new List<Publish>();
        }

    }
}
