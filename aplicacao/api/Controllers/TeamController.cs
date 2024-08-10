using api.Domain.Classes;
using api.Domain.DTOs;
using api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RockBank.Utils;

namespace api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class TeamController
    {


       private readonly ITeamService _teamService;

        public TeamController(ITeamService teamService)
        {
            _teamService = teamService;
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpPost]
        public IResult Create(TeamDTO teamDTO)
        {
            if (teamDTO == null)
            {
                return Results.BadRequest();
            }

            if (!teamDTO.IsValid)
            {
                return Results.ValidationProblem(teamDTO.Notifications.ConvertToProblemDetails());
            }

            Team team = _teamService.Create(teamDTO);

            return Results.Created($"team/{team.Id}", team);
        }

        [HttpGet("{teamId}")]
        public async Task<IResult> Get(Guid teamId) 
        {

            try
            {
                Team team = await _teamService.Get(teamId);

                if (team == null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(team);
            }
            catch (Exception ex)
            {
                return Results.StatusCode(500);
            }

        }

        [HttpGet]
        public async Task<IResult> GetAll()
        {
            List<Team> teams = await _teamService.GetAll();

            return Results.Ok(teams);
        }
    }
}
