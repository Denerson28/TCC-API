using api.Domain.Classes;
using api.Domain.DTOs;
using api.Infra.Data;
using api.Services;
using api.Services.Interfaces;
using ApiTest.Fakes;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiTest.Services
{
    [TestFixture]
    public class TeamServiceTest
    {
        private ApplicationDbContext _context;
        private ITeamService _teamService;
        private DbContextOptions<ApplicationDbContext> _options;

        [SetUp]
        public void Setup()
        {
            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "FakeDatabase")
                .Options;

            _context = new ApplicationDbContext(_options);
            _teamService = new TeamService(_context);

            _context.Teams.RemoveRange(_context.Teams);
            _context.SaveChanges();

            var team = new Team("Development Team");
            _context.Teams.Add(team);
            _context.Users.AddRange(FakeUserData.GetFakeUsers(5));
            _context.SaveChanges();
        }


        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public void Create_ShouldAddTeamToDatabase()
        {
            // Arrange
            var teamDTO = new TeamDTO { Name = "Design Team" };

            // Act
            var team = _teamService.Create(teamDTO);

            // Assert
            Assert.NotNull(team);
            Assert.AreEqual(team.Name, "Design Team");
            Assert.That(_context.Teams.CountAsync().Result, Is.EqualTo(2));
        }

        [Test]
        public async Task Get_ShouldReturnTeamById()
        {
            // Arrange
            var teamDTO = new TeamDTO { Name = "Marketing Team" };
            var team = _teamService.Create(teamDTO);

            // Act
            var fetchedTeam = await _teamService.Get(team.Id);

            // Assert
            Assert.NotNull(fetchedTeam);
            Assert.AreEqual(fetchedTeam.Name, "Marketing Team");
        }

        [Test]
        public async Task GetAll_ShouldReturnAllTeams()
        {
            // Arrange
            _teamService.Create(new TeamDTO { Name = "Team 1" });
            _teamService.Create(new TeamDTO { Name = "Team 2" });

            // Act
            var teams = await _teamService.GetAll();

            // Assert
            Assert.NotNull(teams);
            Assert.AreEqual(teams.Count, 3);
        }
    }
}
