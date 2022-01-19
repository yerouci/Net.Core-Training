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

namespace VL.Services
{
    public class BookService : IBookService
    {
        private ILoggerManager _logger;
        protected VLDBContext dbcontext { get; set; }

        public BookService(VLDBContext _dbcontext, ILoggerManager logger)
        {
            dbcontext = _dbcontext;
            _logger = logger;
        }

        public async Task<Object> GetBooks(BookParameters queryParameters)
        {
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
    }
}
