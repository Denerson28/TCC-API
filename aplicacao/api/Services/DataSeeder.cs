using api.Domain.Classes;
using api.Domain.DTOs;
using api.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace api.Services
{
    public class DataSeeder
    {
        private readonly ApplicationDbContext _context;

        public DataSeeder(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Seed()
        {
            string hashedPassword1 = BCrypt.Net.BCrypt.HashPassword("jardeson123");
            string hashedPassword2 = BCrypt.Net.BCrypt.HashPassword("denerson123");


            var idTeam1 = Guid.NewGuid();
            var idTeam2 = Guid.NewGuid();

            if (!_context.Teams.Any())
            {
                _context.Teams.AddRange(
                    new Team { Name = "Colaboradores", Id = idTeam1 },
                    new Team { Name = "Time Backend", Id = idTeam2 }
                    );

                _context.SaveChanges();

            }

            if (!_context.Users.Any())
            {
                _context.Users.AddRange(
                    new User { Name = "Jardeson", Email = "jardeson@gmail.com", Password = hashedPassword1, UserType = "admin", Role = "ADM", TeamId = idTeam1 },
                    new User { Name = "Denerson", Email = "denerson@gmail.com", Password = hashedPassword2, UserType = "user", Role = "Desenvolvedor Backend", TeamId = idTeam2 }

                );

                _context.SaveChanges();
            }
        }
    }
}
