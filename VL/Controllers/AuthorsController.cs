using Entities.Models;
using Entities.ModelsDTO;
using LoggerService;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VL.Contracts;

namespace VL.Controllers
{
    [ApiController]
    [Route("api/v1.0/library/[controller]")]
    public class AuthorsController : Controller
    {
        protected IAuthorService _authorService { get; set; }
        private ILoggerManager _logger;

        public AuthorsController(IAuthorService authorService, ILoggerManager logger)
        {
            _logger = logger;
            _authorService = authorService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAuthors([FromQuery] AuthorParameters queryParameters)
        {
            var authors = await _authorService.GetAuthors(queryParameters);
            return Ok(authors);
        }

        [HttpPost]
        public async Task<IActionResult> AddAuthor([FromBody] AuthorInputDTO input)
        {
            var author = await _authorService.AddAuthor(input);
            return Ok(author);
        }

        [HttpPost("{authorId}/books")]
        public async Task<IActionResult> AddBook([FromRoute] int authorId, [FromBody] BookInputDTO input)
        {
            var book = await _authorService.AddNewBook(authorId, input);
            return Ok(book);
        }

        [HttpGet("{authorId}")]
        public async Task<IActionResult> GetAuthorDetails([FromRoute] int authorId)
        {
            var author = await _authorService.GetAuthorDetails(authorId);
            return Ok(author);
        }
    }
}
