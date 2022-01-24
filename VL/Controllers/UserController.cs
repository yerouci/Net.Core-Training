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
            _logger.LogInfo($"Response: { data }");
            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> AddNewUser([FromBody] UserInputDTO user)
        {
            var result = await _userService.Add(user);
            _logger.LogInfo($"Response: {result}");
            return Ok(result);
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUserImage([FromRoute] Guid userId, [FromBody] UserImage userImage)
        {
            var result = await _userService.UpdateImage(userId, userImage);
            _logger.LogInfo($"Response: {result}");
            return Ok(result);
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser([FromRoute] Guid userId)
        {
            await _userService.DeleteAsync(userId);
            _logger.LogInfo($"Action: Deleted user with ID: {userId.ToString()}");
            return NoContent();
        }

        [HttpGet("{userId}/subscribe-to-author/{authorId}")]
        public async Task<IActionResult> SubcribeToAuthor([FromRoute] Guid userId, [FromRoute] int authorId)
        {
            await _userService.SubscribeToAuthor(userId, authorId);
            _logger.LogInfo($"Action:  User with ID: {userId.ToString()} has subscribed to the author with ID: {authorId}");
            return Ok();
        }

        [HttpDelete("{userId}/subscribe-to-author/{authorId}")]
        public async Task<IActionResult> DeleteSubcriptionToAuthor([FromRoute] Guid userId, [FromRoute] int authorId)
        {
            await _userService.DeleteSubscriptionToAuthor(userId, authorId);
            _logger.LogInfo($"Action:  User with ID: {userId.ToString()} has unsubscribed to the author with ID: {authorId}");
            return NoContent();
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserById([FromRoute] string userId)
        {
            var result = await _userService.GetById(userId);
            _logger.LogInfo($"Response: {result}");
            return Ok(result);
        }

    }
}
