using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Entities;
using LoggerService;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;
using VL.Contracts;
using VL.Controllers;
using VL.Services;

namespace VLTests
{
    public class BookControllerTests
    {
        private BooksController _booksController;
        private IBookService _booksService;
        
        public BookControllerTests()
        {
            var mockLogger = new Mock<ILoggerManager>();
            var mockMapper = new Mock<IMapper>();

            var dbContextFactory = new Mock<IDbContextFactory<VLDBContext>>();

            var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();
            var options = new DbContextOptionsBuilder<VLDBContext>()
                .UseSqlite(connection).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                .Options;
            _booksService = new BookService(dbContextFactory.Object, mockLogger.Object, mockMapper.Object);
            
            _booksController = new BooksController(_booksService,mockLogger.Object);
            
            //FillDataBase().Wait();
        }
    }
}
