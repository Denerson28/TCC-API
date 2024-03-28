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

        public DbSet<PdfFile> PdfFiles { get; set; }
    }
}
