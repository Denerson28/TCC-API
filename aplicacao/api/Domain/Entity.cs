namespace api.Domain
{
    public abstract class Entity
    {
        public Entity() 
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
