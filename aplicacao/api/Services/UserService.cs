using api.Domain.Classes;
using api.Domain.DTOs;
using api.Infra.Data;
using api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace api.Services
{
    public class UserService : IUserService
    {

        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User> Create(UserDTO userDTO)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    User user = new User(userDTO.Name, userDTO.Email, userDTO.Password, userDTO.Role, userDTO.TeamId);

                    _context.Users.Add(user);

                    var team = await _context.Teams.Include(t => t.Users).FirstOrDefaultAsync(t => t.Id == userDTO.TeamId);
                    if (team != null)
                    {
                        team.Users.Add(user);
                    }
                    else
                    {
                        throw new Exception("Team not found");
                    }

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return user; // Retorna o novo usuário criado
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw; // Re-lança a exceção para ser tratada externamente
                }
            }
        }

        public User Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public List<User> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
