using AutoMapper;
using Entities;
using Entities.Models;
using Entities.ModelsDTO;
using System.Threading.Tasks;
using VL.Contracts;
using VL.Exceptions;
using System.Net;
using System.Linq;
using LoggerService;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace VL.Services
{
    public class AuthorService : IAuthorService
    {

        protected VLDBContext _dbcontext { get; set; }
        private readonly IMapper _mapper;
        private readonly ILoggerManager _logger;

        public AuthorService(VLDBContext dbcontext, IMapper mapper, ILoggerManager logger)
        {
            _dbcontext = dbcontext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PagedList<Author>> GetAuthors(AuthorParameters queryParameters)
        {
            return await PagedList<Author>.ToPagedList(_dbcontext.Authors,
                queryParameters.Offset,
                queryParameters.Limit);
        }

        public async Task<Author> AddAuthor(AuthorInputDTO input) 
        {
            try
            {
                var author = _mapper.Map<Author>(input);

                var entity = _dbcontext.Authors.Add(author);
                await _dbcontext.SaveChangesAsync();

                return entity.Entity;
            }
            catch (System.Exception e)
            {
                _logger.LogError(e.Message);
                throw new RestException(HttpStatusCode.InternalServerError,
                    "An unexpected error occurred while trying to perform the operation: Add New Author.");
            }            
        }

        public async Task<AuthorDetailsDTO> GetAuthorDetails(int id) 
        {
            var author = await _dbcontext.Authors
                .Include(i=>i.Users)
                .FirstOrDefaultAsync(f => f.Id.Equals(id));            

            if (author == null)
            {
                throw new RestException(HttpStatusCode.NotFound,
                    $"The provided identifier: {id} does not match any author.");
            }

            try
            {
                var details = _mapper.Map<AuthorDetailsDTO>(author);

                var books = _dbcontext.Books.Where(w => w.Author.Id.Equals(id)).ToList();

                details.Books = _mapper.Map<List<BookDetailsDTO>>(books);

                return details;
            }
            catch (System.Exception e)
            {
                _logger.LogError(e.Message);
                throw new RestException(HttpStatusCode.InternalServerError,
                    "An unexpected error occurred while trying to perform the operation: Get Author Details.");
            }            
        }

        public async Task<BookDTO> AddNewBook(int id, BookInputDTO input)
        {
            var book = _mapper.Map<Book>(input);

            var author = _dbcontext.Authors.Find(id);
            if (author == null)
            {
                throw new RestException(HttpStatusCode.NotFound,
                    $"The provided identifier: {id} does not match any author.");
            }

            book.Author = author;

            try
            {
                var result = _dbcontext.Books.Add(book);
                await _dbcontext.SaveChangesAsync();

                return _mapper.Map<BookDTO>(result.Entity);
            }
            catch (System.Exception e) 
            {
                _logger.LogError(e.Message);
                throw new RestException(HttpStatusCode.InternalServerError,
                    "An unexpected error occurred while trying to perform the operation: Add New Book.");
            }
        }
    }
}
