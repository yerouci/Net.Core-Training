using Entities.Models;
using Entities.ModelsDTO;
using LoggerService;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using VL.Contracts;

namespace VL.Controllers
{
    [ApiController]
    [Route("api/v1.0/library/[controller]")]
    public class UsersController : Controller
    {
        private IUserService _userService;
        private ILoggerManager _logger;

        public UsersController(IUserService userService, ILoggerManager logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers([FromQuery] UserParameters stringParameters)
        {
            var data = await _userService.GetAllUsers(stringParameters);
            _logger.LogInfo($"Response: { data } ");
            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> AddNewUser([FromBody] UserInputDTO user)
        {
            var result = await _userService.Add(user);
            return Ok(result);
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUserImage([FromRoute] Guid userId, [FromBody] UserImage userImage)
        {
            var result = await _userService.UpdateImage(userId, userImage);
            return Ok(result);
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser([FromRoute] Guid userId)
        {
            await _userService.DeleteAsync(userId);
            return NoContent();
        }

        [HttpGet("{userId}/subscribe-to-author/{authorId}")]
        public async Task<IActionResult> SubcribeToAuthor([FromRoute] Guid userId, [FromRoute] int authorId)
        {
            await _userService.SubscribeToAuthor(userId, authorId);
            return Ok();
        }

    }
}
