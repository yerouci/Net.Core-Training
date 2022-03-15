using Entities;
using Entities.Filters;
using Entities.Models;
using Entities.ModelsDTO;
using LoggerService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VL.Contracts;
using System.Linq.Dynamic.Core;
using AutoMapper;
using System.Net;
using VL.Exceptions;



namespace VL.Services
{
    public class BookService : IBookService
    {
        private ILoggerManager _logger;
        private readonly IMapper _mapper;
        IDbContextFactory<VLDBContext> _dbContextFactory;

        public BookService(IDbContextFactory<VLDBContext> contextFactory, ILoggerManager logger, IMapper mapper)
        {
            _dbContextFactory = contextFactory;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Object> GetBooks(BookParameters queryParameters)
        {
            try
            {
                await using var dbcontext = _dbContextFactory.CreateDbContext();
                var query = dbcontext.Books.Select(s => new BookDTO
                {
                    Title = s.Title,
                    AuthorName = s.Author.Name,
                    EditorialName = s.EditorialName,
                    ISBN = s.ISBN,
                    AuthorId = s.Author.Id,
                    Date = s.Date,
                    Qualification = s.Qualification
                }).Where(FilterBooks.GetFiltersExpression(queryParameters));

                /*Apply sort if needed*/
                var orderQueryBuilder = new StringBuilder();
                string sortFiledName = "Qualification";
                if (queryParameters.Sort != null)
                {
                    var sortingOrder = queryParameters.Sort.Value ? "ascending" : "descending";
                    orderQueryBuilder.Append($"{sortFiledName} {sortingOrder} ");
                    query = query.OrderBy(orderQueryBuilder.ToString());
                }

                /*Logging the query to evaluate efficiency*/
                _logger.LogInfo($"Query: {query.ToQueryString() } ");

                var booksResult = await PagedList<BookDTO>.ToPagedList(query,
                queryParameters.Offset,
                queryParameters.Limit);

                return new ResponseDto<BookPaginatedDto>
                {
                    TotalCount= booksResult.TotalCount,
                    PageSize = booksResult.PageSize,
                    CurrentPage = booksResult.CurrentPage,
                    TotalPages = booksResult.TotalPages,
                    HasNext = booksResult.HasNext,
                    HasPrevious = booksResult.HasPrevious,
                    Data = booksResult.Select(s => new BookPaginatedDto//Querying the object to send only the necessary fields to the front
                    {
                        Title= s.Title,
                        AuthorName = s.AuthorName,
                        EditorialName = s.EditorialName,
                         ISBN = s.ISBN
                    })
                };
            }
            catch (System.Exception e)
            {
                _logger.LogError(e.Message);
                throw new RestException(HttpStatusCode.InternalServerError,
                "An unexpected error occurred while trying to perform the operation: Get All Books.");
            }

        }




        public async Task<Object> GetReviews(ReviewsParameters queryParameters, int id)
        {
            try
            {
                await using var dbcontext = _dbContextFactory.CreateDbContext();
                var query = dbcontext.Reviews.Where(w => dbcontext.Books.Any(b => b.Id == id && b.Reviews.Any(a => a.Id == w.Id))).Select(s => new ReviewDTO
                {
                    Id = s.Id,
                    BookId = id,
                    Opinion = s.Opinion,
                    UserId = s.User.Id,
                    Date = s.Date,
                    Qualification = (int)s.Qualification
                }).Where(FilterReviews.GetFiltersExpression(queryParameters));

                /*Apply sort if needed*/
                var orderQueryBuilder = new StringBuilder();
                string sortFiledName = "Date";
                if (queryParameters.Sort != null)
                {
                    var sortingOrder = queryParameters.Sort.Value ? "ascending" : "descending";
                    orderQueryBuilder.Append($"{sortFiledName} {sortingOrder} ");

                    query = query.OrderBy(orderQueryBuilder.ToString());
                }
                
                /*Logging the query to evaluate efficiency*/
                _logger.LogInfo($"Query: {query.ToQueryString() } ");

                var reviewsResult = await PagedList<ReviewDTO>.ToPagedList(query,
                queryParameters.Offset,
                queryParameters.Limit);

                return new
                {
                    reviewsResult.TotalCount,
                    reviewsResult.PageSize,
                    reviewsResult.CurrentPage,
                    reviewsResult.TotalPages,
                    reviewsResult.HasNext,
                    reviewsResult.HasPrevious,
                    rows = reviewsResult
                };
            }
            catch (System.Exception e)
            {
                _logger.LogError(e.Message);
                throw new RestException(HttpStatusCode.InternalServerError,
                "An unexpected error occurred while trying to perform the operation: Get Reviews.");
            }

        }

        public async Task<Review> AddReview(int bookId, string userId, ReviewInputDTO input)
        {

            var review = _mapper.Map<Review>(input);
            await using var dbcontext = _dbContextFactory.CreateDbContext();
            var user = dbcontext.Users.FirstOrDefault(a => a.Id.Equals(Guid.Parse(userId)));
            if (user == null)
            {
                _logger.LogError($"Error: An unexpected error occurred at trying to get user with ID: {userId.ToString()}.");
                throw new RestException(HttpStatusCode.NotFound,
                $"The provided identifier: {userId} does not match any user.");
            }

            review.Date = DateTime.UtcNow;
            review.User = user;
            try
            {
                var result = dbcontext.Reviews.Add(review);

                var book = dbcontext.Books
                .Include(i => i.Reviews)
                .FirstOrDefault(w => w.Id.Equals(bookId));

                book.Reviews.Add(result.Entity);
                book.Qualification = CalculateQualification(book.Reviews);
                dbcontext.Books.Update(book);

                await dbcontext.SaveChangesAsync();

                return result.Entity;
            }
            catch (System.Exception e)
            {
                _logger.LogError(e.Message);
                throw new RestException(HttpStatusCode.InternalServerError,
                "An unexpected error occurred while trying to perform the operation: Add Review.");
            }
        }

        private int CalculateQualification(ICollection<Review> reviews)
        {
            int aux = 0;
            foreach (var review in reviews)
            {
                aux += (int)review.Qualification;
            }
            return aux / reviews.Count;
        }

    }
}