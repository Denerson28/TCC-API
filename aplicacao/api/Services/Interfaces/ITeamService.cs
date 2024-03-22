using api.Domain.Classes;
using api.Domain.DTOs;

namespace api.Services.Interfaces
{
    public interface ITeamService
    {
        public Task<List<Team>> GetAll();
        public Task<Team> Get(Guid id);
        public Team Create(TeamDTO teamDTO);
    }
}
