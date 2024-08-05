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

            // Configurar a chave primária da entidade User
            builder.Entity<User>()
                .HasKey(u => u.Id);

            // Configurar a chave primária da entidade Team
            builder.Entity<Team>()
                .HasKey(t => t.Id);

            //Configurar a chave primária da entidade Recommend
            builder.Entity<Recommend>()
                .HasKey(r => r.Id);

            builder.Entity<Publish>().ToTable("Publishes");
            builder.Entity<Publish>().HasKey(p => p.Id);


            // Configurar relacionamento um-para-muitos entre Team e User
            builder.Entity<Team>()
                        .HasMany(t => t.Users)             // Um time pode ter muitos usuários
                        .WithOne();                        // Um usuário pertence a um time

            // Define a relação entre Publish e User
            builder.Entity<User>()
                        .HasMany(u => u.Publishes)          // Um Usuário pode ter várias publicações
                        .WithOne();                        // O Pdf pertence a um usuário

            // Define a relação entre Recommend e User
            builder.Entity<User>()
                        .HasMany(u => u.RecommendsReceived) // Um Usuário pode receber várias recomendações
                        .WithOne();                        // A recomendação pertence a um usuário
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configuration)
        {
            configuration.Properties<string>()
                .HaveMaxLength(100);
        }
    }
}
