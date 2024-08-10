using api.Domain.DTOs;
using api.Infra.Data;
using api.Services;
using api.Services.Interfaces;
using ApiTest.Fakes;
using Microsoft.EntityFrameworkCore;


namespace ApiTest.Services
{
    [TestFixture]
    public class PublishServiceTest
    {
        private ApplicationDbContext _context;
        private IPublishService _publishService;
        private DbContextOptions<ApplicationDbContext> _options;
  
        [SetUp]
        public void Setup()
        {
            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "FakeDatabase")
                .Options;

            _context = new ApplicationDbContext(_options);
            _publishService = new PublishService(_context);



            _context.Users.Add(FakeUserData.GetFakeUser());
            _context.SaveChanges();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }


        [Test]
        public async Task Upload_ShouldSavePublishToDatabaseAsync()
        {
            // Arrange
            PublishRequestDTO publish = new PublishRequestDTO
            {
                Title = "TestFile",
                Image = "TestFile",
                Description = "TestFile"
            };

            // Act
            PublishResponseDTO response =  await _publishService.Upload(FakeUserData.GetFakeUser().Id, publish);

            // Assert
            Assert.NotNull(response);
            Assert.That(response.Title, Is.EqualTo("TestFile"));


        }
    }
}
