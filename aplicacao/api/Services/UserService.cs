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
            using (var register = _context.Database.BeginTransaction())
            {

                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(userDTO.Password);
                try
                {
                    User user = new User(userDTO.Name, userDTO.Email, hashedPassword,userDTO.UserType, userDTO.Role, userDTO.TeamId);

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
                    await register.CommitAsync();

                    return user; // Retorna o novo usuário criado
                }
                catch (Exception ex)
                {
                    await register.RollbackAsync();
                    throw; // Re-lança a exceção para ser tratada externamente
                }
            }
        }

        public async Task<User> Get(Guid id)
        {

            User user = _context.Users.Find(id);


            return await _context.Users
                .Include(r => r.RecommendsReceived)
                .Include(p => p.Publishes)
                .FirstOrDefaultAsync(u => u.Id == id);
                //.Include(p => p.Publishes)
                //.FirstOrDefaultAsync(u => u.Id == id);
        }

        public List<User> GetAll()
        {
            return _context.Users.ToList();
        }

        public async Task<User> Update(Guid userId, UserDTO userDTO)
        {

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(userDTO.Password);

            var existingUser = await _context.Users.FindAsync(userId);

            if (existingUser == null)
            {
                throw new Exception("Usuário não encontrado");
            }

            // Atualiza os campos do usuário com os novos valores
            existingUser.Name = userDTO.Name;
            existingUser.Email = userDTO.Email;
            existingUser.Password = hashedPassword;
            existingUser.UserType = userDTO.UserType;
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
            return BCrypt.Net.BCrypt.Verify(password, user.Password);
        }
    }
}
