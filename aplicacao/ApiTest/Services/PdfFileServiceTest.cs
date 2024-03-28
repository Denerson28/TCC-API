using api.Domain.Classes;
using api.Infra.Data;
using api.Services;
using api.Services.Interfaces;
using ApiTest.Fakes;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiTest.Services
{
    [TestFixture]
    public class PdfFileServiceTest
    {
        private ApplicationDbContext _context;
        private IPdfFileService _pdfFileService;
        private DbContextOptions<ApplicationDbContext> _options;


        [SetUp]
        public void Setup()
        {
            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "FakeDatabase")
                .Options;

            _context = new ApplicationDbContext(_options);
            _pdfFileService = new PdfFileService(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }


        [Test]
        public async Task Upload_ShouldSavePdfToDatabaseAsync()
        {
            // Arrange
            string directoryPath = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = Path.Combine(directoryPath, "../../../Services/Archives/testeMtoByte.pdf");
            byte[] fileBytes = null;

            try
            {
                //Verifica se o arquivo existe
                if (!File.Exists(filePath))
                {
                    throw new FileNotFoundException($"O arquivo \"{filePath}\" não foi encontrado.");
                }

                // Lê o arquivo como bytes
                fileBytes = File.ReadAllBytes(filePath);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao ler o arquivo PDF: {ex.Message}");
            }

            var pdf = new PdfFile
            {
                Name = "TestFile.pdf",
                Content = fileBytes,
                UserId = new Guid(),
                Description = "Description"
            };


            // Act
            await _pdfFileService.Upload(pdf);

            // Assert
            var savedPdf = await _context.PdfFiles.FirstOrDefaultAsync();
            Assert.NotNull(savedPdf);
            Assert.AreEqual("TestFile.pdf", savedPdf.Name);
            // Adicione mais verificações conforme necessário
            

        }
    }
}
