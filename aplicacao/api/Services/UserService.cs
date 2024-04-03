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
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw; // Re-lança a exceção para ser tratada externamente
                }
            }
        }

        public User Get(Guid id)
        {

            User user = _context.Users.Find(id);

            return user;
        }

        public List<User> GetAll()
        {
            return _context.Users.ToList();
        }

        public async Task<User> Update(Guid userId, UserDTO userDTO)
        {
            var existingUser = await _context.Users.FindAsync(userId);

            if (existingUser == null)
            {
                throw new Exception("Usuário não encontrado");
            }

            // Atualiza os campos do usuário com os novos valores
            existingUser.Name = userDTO.Name;
            existingUser.Email = userDTO.Email;
            existingUser.Password = userDTO.Password;
            existingUser.Role = userDTO.Role;

            await _context.SaveChangesAsync();

            return existingUser;
        }

        public User GetUserByEmail(string email)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);

            if (user == null)
            {
                throw new Exception("Usuário não encontrado");
            }

            return user;
        }

        public bool CheckPassword(Guid userId, string password)
        {
            var user = _context.Users.Find(userId);

            if (user == null)
            {
                throw new Exception("Usuário não encontrado");
            }

            // Verifica se a senha fornecida corresponde à senha armazenada
            return user.Password == password;
        }
    }
}
