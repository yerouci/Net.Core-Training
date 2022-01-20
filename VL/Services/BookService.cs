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

namespace VL.Services
{
    public class BookService : IBookService
    {
        private ILoggerManager _logger;
        protected VLDBContext _dbcontext { get; set; }
        private readonly IMapper _mapper;

        public BookService(VLDBContext dbcontext, ILoggerManager logger, IMapper mapper)
        {
            this._dbcontext = dbcontext;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Object> GetBooks(BookParameters queryParameters)
        {
            var query = _dbcontext.Books.Select(s => new BookDTO
            {
                Title = s.Title,
                AuthorName = s.Author.Name,
                EditorialName = s.EditorialName,
                ISBN = s.ISBN,
                AuthorId = s.Author.Id,
                Date = s.Date,
                Qualification = s.Qualification
            }).Where(FilterBooks.GetFiltersExpression(queryParameters));

            /*Apply filters if needed*/
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
            
            return new {
                booksResult.TotalCount,
                booksResult.PageSize,
                booksResult.CurrentPage,
                booksResult.TotalPages,
                booksResult.HasNext,
                booksResult.HasPrevious,
                rows = booksResult.Select(s => new //Querying the object to send only the necessary fields to the front
                {
                    s.Title,
                    s.AuthorName,
                    s.EditorialName,
                    s.ISBN
                })
            };
        }


        public async Task<bool> AddReview(int bookId, string userId, ReviewInputDTO input) 
        {
            var review = _mapper.Map<Review>(input);

            var user = _dbcontext.Users.Where(w => w.Id.Equals(userId)).FirstOrDefault();

            review.Date = DateTime.UtcNow;
            review.User = user;

            var result = await _dbcontext.Reviews.AddAsync(review);

            var book = _dbcontext.Books.Find(bookId);
            
            if (book.Reviews.ToList() == null)
            {
                book.Reviews = new List<Review>();
            }
            book.Reviews.Add(result.Entity);
            book.Qualification = CalculateQualification(book.Reviews);
            _dbcontext.Books.Update(book);

            _dbcontext.SaveChanges();

            return true;
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
