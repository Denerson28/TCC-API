using api.Domain.Classes;
using api.Domain.DTOs;
using api.Infra.Data;
using api.Services;
using api.Services.Interfaces;
using ApiTest.Fakes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiTest.Services
{
    [TestFixture]
    public class UserServiceTest
    {
        private ApplicationDbContext _context;
        private IUserService _userService;
        private DbContextOptions<ApplicationDbContext> _options;

        [SetUp]
        public void Setup()
        {
            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
                            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                            .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                            .Options;

            _context = new ApplicationDbContext(_options);
            _userService = new UserService(_context);

            var team = new Team("Development Team");
            _context.Teams.Add(team);

            var users = FakeUserData.GetFakeUsers(5);
            foreach (var user in users)
            {
                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            }

            _context.Users.AddRange(users);
            _context.SaveChanges();
        }


        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task Create_ShouldAddUserToDatabase()
        {
            // Arrange
            var userDTO = new UserRequestDTO
            {
                Name = "New User",
                Email = "newuser@example.com",
                Password = "Password123",
                Photo = "image",
                UserType = "Standard",
                Role = "Developer",
                TeamId = _context.Teams.First().Id,
                TeamName = "Development Team"
            };

            // Act
            var userResponse = await _userService.Create(userDTO);

            // Assert
            Assert.NotNull(userResponse);
            Assert.AreEqual(userResponse.Name, "New User");
            Assert.AreEqual(userResponse.Email, "newuser@example.com");
            Assert.That(_context.Users.CountAsync().Result, Is.EqualTo(6)); // 5 do setup + 1 novo
        }

        [Test]
        public async Task Get_ShouldReturnUserById()
        {
            // Arrange
            var user = _context.Users.First();

            // Act
            var userResponse = await _userService.Get(user.Id);

            // Assert
            Assert.NotNull(userResponse);
            Assert.AreEqual(userResponse.Name, user.Name);
        }

        [Test]
        public void GetAll_ShouldReturnAllUsers()
        {
            // Act
            var users = _userService.GetAll();

            // Assert
            Assert.NotNull(users);
            Assert.AreEqual(users.Count, 5); // 5 do setup
        }

        [Test]
        public async Task Update_ShouldModifyUserInDatabase()
        {
            // Arrange
            var user = _context.Users.First();
            var userUpdateDTO = new UserUpdateDTO
            {
                Name = "Updated User",
                Photo = "image"
            };

            // Act
            var updatedUser = await _userService.Update(user.Id, userUpdateDTO);

            // Assert
            Assert.NotNull(updatedUser);
            Assert.AreEqual(updatedUser.Name, "Updated User");
            Assert.AreEqual(updatedUser.Photo, "image");
        }

        [Test]
        public void GetUserByEmail_ShouldReturnCorrectUser()
        {
            // Arrange
            var user = _context.Users.First();

            // Act
            var fetchedUser = _userService.GetUserByEmail(user.Email);

            // Assert
            Assert.NotNull(fetchedUser);
            Assert.AreEqual(fetchedUser.Email, user.Email);
        }

        [Test]
        public void CheckPassword_ShouldReturnTrueForCorrectPassword()
        {
            // Arrange
            var user = _context.Users.First();
            var password = "hashedpassword123";

            // Act
            var result = _userService.CheckPassword(user.Id, password);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task GetUsersRanking_ShouldReturnUsersOrderedByStars()
        {
            // Act
            var ranking = await _userService.GetUsersRanking();

            // Assert
            Assert.NotNull(ranking);
        }
    }
}
