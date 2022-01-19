using Entities.Models;
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
    }
}
