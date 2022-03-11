using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Entities;
using Entities.Models;
using GenFu;
using LoggerService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using VL;
using VL.Contracts;
using VL.Controllers;
using VL.Services;
using Xunit;

namespace VLTests
{
    public class UserControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {

        #region Configuring DBContext

        private readonly WebApplicationFactory<Startup> _factory;
        private readonly VLDBContext _context;


        public UserControllerTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory.WithWebHostBuilder(config =>
            {
                config.ConfigureServices(services =>
                {
                    services.AddDbContext<VLDBContext>(options => { options.UseInMemoryDatabase("InMemory"); });
                });
            });

            var dbContexOoptions = new DbContextOptionsBuilder<VLDBContext>().UseInMemoryDatabase("VlDatabase").Options;
            _context = new VLDBContext(dbContexOoptions);
            _context.Database.EnsureCreated();
           
        }

        #endregion



        #region CRUD_UserControllerTests
        [Fact]
        public async Task Delete_ReturnsHttpNotFound_ForInvalidUserId()
        {
            // Arrange
            var mockUserService = new Mock<IUserService>();
            var mockLogger = new Mock<ILoggerManager>();
            var controller = new UsersController(mockUserService.Object,mockLogger.Object);

            // Act
            var result = await controller.DeleteUser(Guid.Empty);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }
        #endregion

        #region List and Searchs

        [Fact]
        public async Task GetUsersTest()
        {
            // arrange
            var mockMapper = new Mock<IMapper>();
            var mockLogger = new Mock<ILoggerManager>();
            var mockConetxFactory = new Mock<IDbContextFactory<VLDBContext>>();
            mockConetxFactory.Setup(m=>m.CreateDbContext()).Returns(_context);

            //var context = CreateDbContext();

            var service = new UserService(mockConetxFactory.Object, mockMapper.Object, mockLogger.Object);

            var controller = new UsersController(service,mockLogger.Object);

            // act
            var results = await controller.GetAllUsers(new UserParameters(){Limit = 50, Offset = 0});

            // count =  results ;

            // assert
            var okResult = Assert.IsType<OkObjectResult>(results);
            var returnValue = Assert.IsType<List<User>>(okResult.Value);
            Assert.Equal(5, returnValue.Count);
            Assert.Equal(10, 10);
        }

        #endregion

        #region DBContext and FakeData

        private Mock<VLDBContext> CreateDbContext1()
        {
            var persons = GetFakeData().AsQueryable().BuildMockDbSet();

            //var dbSet = new Mock<DbSet<User>>();
            //dbSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(persons.Provider);
            //dbSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(persons.Expression);
            //dbSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(persons.ElementType);
            //dbSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(persons.GetEnumerator());

            var context = new Mock<VLDBContext>
            {
                Object =
                {
                    Users = persons.Object
                }
            };

            //context.Setup(c => c.Users).Returns(dbSet.Object);

            return context;
        }

        private IEnumerable<User> GetFakeData()
        {
            var persons = GenFu.GenFu.ListOf<User>(10);
            persons.ForEach(x => x.Id = Guid.NewGuid());
            return persons.Select(_ => _);
        }

        #endregion
    }
}
