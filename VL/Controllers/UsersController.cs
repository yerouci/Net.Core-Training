using LoggerService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VL.Controllers
{
    [ApiController]
    [Route("api/v1.0/library/[controller]")]
    public class UsersController : Controller
    {
        private ILoggerManager _logger;

        public UsersController(ILoggerManager logger)
        {
            _logger = logger;            
        }

        [HttpGet]
        public async Task<JsonResult> GetAllUsers()
        {
            await Task.CompletedTask;

            return Json(new { success = true });
        }
    }
}
