using api.Domain.Classes;
using api.Infra.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiTest.Fakes
{

    public class FakeDbContext : DbContext
    {
        public FakeDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Publish> Publishes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Recommend> Recommends { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Team> Teams { get; set; }

    }
}
