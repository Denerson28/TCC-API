using api.Domain.Classes;
using api.Domain.DTOs;
using api.Infra.Data;
using api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace api.Services
{
    public class TeamService : ITeamService
    {

        private readonly ApplicationDbContext _context;

        public TeamService(ApplicationDbContext context)
        {
            _context = context;
        }
        public Team Create(TeamDTO teamDTO)
        {
            Team team = new Team(teamDTO.Name);

            _context.Add(team);
            _context.SaveChanges();

            return team;
        }

        public async Task Delete(Guid id)
        {
            Team team = await Get(id);

            _context.Remove(team);
            _context.SaveChanges();
        }

        public async Task<Team> Get(Guid id)
        {
            return await _context.Teams
                .Include(t => t.Users)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<List<Team>> GetAll()
        {
            return await _context.Teams.ToListAsync();
        }

        public async Task Update(Guid teamId, TeamDTO teamDTO)
        {
            Team team = await Get(teamId);

            team.Name = teamDTO.Name;

            _context.Update(team);
            _context.SaveChanges();
        }
    }
}
