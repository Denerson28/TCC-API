using api.Domain.Classes;
using Microsoft.EntityFrameworkCore;

namespace api.Infra.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Publish> Publishes { get; set; }
        public DbSet<Recommend> Recommendations { get; set; }

        public DbSet<Feedback> Feedbacks { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>()
                .Property(p => p.Name).IsRequired();
            builder.Entity<User>()
                .Property(p => p.Password).IsRequired();
            builder.Entity<User>()
                .Property(p => p.Role).IsRequired();
            builder.Entity<User>()
                .Property(p => p.TeamId).IsRequired();
            builder.Entity<User>()
                .Property(p => p.Email).IsRequired();
            builder.Entity<User>()
                .Property(p => p.UserType).IsRequired();

            builder.Entity<User>()
                .HasKey(u => u.Id);

            builder.Entity<Team>()
                .HasKey(t => t.Id);

            builder.Entity<Recommend>()
                .HasKey(r => r.Id);

            builder.Entity<Feedback>()
                .HasKey(f => f.Id);

            builder.Entity<Publish>().ToTable("Publishes");
            builder.Entity<Publish>().HasKey(p => p.Id);


            builder.Entity<Team>()
                        .HasMany(t => t.Users)             
                        .WithOne();

            
            builder.Entity<User>()
                        .HasMany(u => u.Publishes) 
                        .WithOne(); 

            
            builder.Entity<User>()
                        .HasMany(u => u.RecommendsReceived) 
                        .WithOne();                        
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configuration)
        {
            configuration.Properties<string>()
                .HaveMaxLength(100);
        }
    }
}
