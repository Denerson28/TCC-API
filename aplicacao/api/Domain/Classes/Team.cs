namespace api.Domain.Classes
{
    public class Team : Entity
    {
        public ICollection<User> Users { get; set; }

        public string Name { get; set; }
        public Team()
        {
            Users = new List<User>();
        }

        public Team(string name)
        {
            Users = new List<User>();
            Name = name;
        }
    }
}
