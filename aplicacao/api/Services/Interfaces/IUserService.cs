using api.Domain.Classes;
using api.Domain.DTOs;

namespace api.Services.Interfaces
{
    public interface IUserService
    {
        public List<User> GetAll();
        public Task<User> Get(Guid id);
        public Task<User> Create(UserDTO userDTO);
        public Task<User> Update(Guid Id,UserDTO userDTO);
        public User GetUserByEmail(string email);
        public bool CheckPassword(Guid userId, string password);
    }
}
