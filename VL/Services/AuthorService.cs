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
using System.Linq.Dynamic.Core;
using MailingService.Contracts;
using Microsoft.Extensions.Options;
using MailingService;

namespace VL.Services
{
    public class AuthorService : IAuthorService
    {

        protected VLDBContext _dbcontext { get; set; }
        private readonly IMapper _mapper;
        private readonly ILoggerManager _logger;
        private readonly IEmailSender _emailService;
        private readonly IOptions<EmailDefinition> _emailSettings;

        public AuthorService(VLDBContext dbcontext, IMapper mapper, ILoggerManager logger, IEmailSender emailService, IOptions<EmailDefinition> emailSettings)
        {
            _dbcontext = dbcontext;
            _mapper = mapper;
            _logger = logger;
            _emailService = emailService;
            _emailSettings = emailSettings;
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

                /*Send notification email to all user suscribed*/
                var usersSuscribed = await _dbcontext.Authors.Where(w => w.Id == id).Select(s => new { userEmailsList = s.Users.Select(u => u.Email) }).ToListAsync();

                if(usersSuscribed.Count > 0)
                {
                    foreach (var userEmail in usersSuscribed[0].userEmailsList)
                    {
                        //Calling email sending simulator
                        await _emailService.SendEmailAsync(_emailSettings.Value.SenderEmail, userEmail, _emailSettings.Value.Subject, "This is an Automatic email generated for the system in order to notify you of new book release.");
                    }
                }

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
