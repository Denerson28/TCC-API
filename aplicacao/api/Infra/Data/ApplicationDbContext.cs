﻿using api.Domain;
using api.Domain.Classes;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace api.Infra.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<PdfFile> PdfFiles { get; set; }

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
                .Property(c => c.Name).IsRequired();

            // Configurar a chave primária da entidade User
            builder.Entity<User>()
                .HasKey(u => u.Id);

            // Configurar a chave primária da entidade Team
            builder.Entity<Team>()
                .HasKey(t => t.Id);

            builder.Entity<PdfFile>().ToTable("PdfFiles");
            builder.Entity<PdfFile>().HasKey(p => p.Id);


            // Configurar relacionamento um-para-muitos entre Team e User
            builder.Entity<Team>()
                        .HasMany(t => t.Users)             // Um time pode ter muitos usuários
                        .WithOne();                        // Um usuário pertence a um time

            // Define a relação entre PdfFile e User
            builder.Entity<User>()
                        .HasMany(u => u.PdfFiles)          // Um Usuário pode ter vários Pdfs
                        .WithOne();                        // O Pdf pertence a um usuário
        }

    protected override void ConfigureConventions(ModelConfigurationBuilder configuration)
        {
            configuration.Properties<string>()
                .HaveMaxLength(100);
        }
    }
}
