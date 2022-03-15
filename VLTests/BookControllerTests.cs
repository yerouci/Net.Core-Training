using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Entities;
using Entities.Models;
using Entities.ModelsDTO;
using LoggerService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;
using VL.Contracts;
using VL.Controllers;
using VL.Services;
using Xunit;

namespace VLTests
{
    public class BookControllerTests
    {
        private BooksController _booksController;
        private IBookService _booksService;
        private readonly VLDBContext _dbContext;

        public BookControllerTests()
        {
            var mockLogger = new Mock<ILoggerManager>();
            var mockMapper = new Mock<IMapper>();


            var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();
            var options = new DbContextOptionsBuilder<VLDBContext>()
                .UseSqlite(connection).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                .Options;

            _dbContext = new VLDBContext(options);
            var dbContextFactory = new Mock<IDbContextFactory<VLDBContext>>();
            dbContextFactory.Setup(m => m.CreateDbContext()).Returns(_dbContext);

            _booksService = new BookService(dbContextFactory.Object, mockLogger.Object, mockMapper.Object);

            _booksController = new BooksController(_booksService, mockLogger.Object);

            FillDataBase().Wait();
        }

        [Fact]
        public async Task GetAllBook_Return_OkObjectResult()
        {
            //Arrange
            var mockLogger = new Mock<ILoggerManager>();
            var mockBookService = new Mock<IBookService>();
            var mockQueryParams = new Mock<BookParameters>();
            var controller = new BooksController(mockBookService.Object, mockLogger.Object);
            //Act
            var books = await controller.GetAllBooks(mockQueryParams.Object);
            
            //Assert
            Assert.IsType<OkObjectResult>(books);
        }

        [Fact]
        public async Task GetAllBook_Return_ResponseDto()
        {
            //Arrange
            var mockQueryParams = new BookParameters();

            //Act
            var books = await _booksController.GetAllBooks(mockQueryParams);

            //Assert
            var result = Assert.IsType<OkObjectResult>(books);
            Assert.IsType<ResponseDto<BookPaginatedDto>>(result.Value);
        }

        private async Task FillDataBase()
        {
            ///Falta codigo aqui
            _dbContext.Database.EnsureCreated();
            await _dbContext.SaveChangesAsync();
        }
    }
}
