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
    public class BooksController : Controller
    {
        protected IBookService _bookService { get; set; }
        private ILoggerManager _logger;

        public BooksController(IBookService bookService, ILoggerManager logger)
        {
            _logger = logger;
            _bookService = bookService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBooks([FromQuery] BookParameters queryParameters)
        {
            var authors = await _bookService.GetBooks(queryParameters);

            _logger.LogInfo($"Response: { authors } ");

            return Ok(authors);
        }
    }
}