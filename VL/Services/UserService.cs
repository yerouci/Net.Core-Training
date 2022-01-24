using AutoMapper;
using AutoMapper.QueryableExtensions;
using Entities;
using Entities.Models;
using Entities.ModelsDTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VL.Contracts;
using VL.Exceptions;
using System.Net;
using LoggerService;

namespace VL.Services
{
    public class UserService : IUserService
    {
        protected VLDBContext _dbcontext { get; set; }
        private readonly IMapper _mapper;
        private readonly ILoggerManager _logger;

        public UserService(VLDBContext dbcontext, IMapper mapper, ILoggerManager logger)
        {
            _dbcontext = dbcontext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<User> Add(UserInputDTO input)
        {
            try
            {
                User item = _mapper.Map<User>(input);

                item.Id = Guid.NewGuid();
                item.CreatedAt = DateTime.UtcNow;

                var entity = await _dbcontext.Users.AddAsync(item);
                _dbcontext.SaveChanges();

                return entity.Entity;
            }
            catch(System.Exception e) 
            {
                _logger.LogError(e.Message);
                throw new RestException(HttpStatusCode.InternalServerError, "An unexpected error occurred while trying to perform the operation: Add New User.");
            }
        }

        public async Task<User> GetById(Guid userId)
        {
            var response = await _dbcontext.Users.FindAsync(userId);

            if (response == null)
            {
                _logger.LogError($"Error: An unexpected error occurred at trying to get user with ID: {userId.ToString()}.");
                throw new RestException(HttpStatusCode.NotFound, 
                    $"The provided identifier: {userId} does not match any user.");
            }
            return response;
        }

        private async Task<User> GetUserById(Guid userId)
        {
            var response = await _dbcontext.Users
                .Where(w => w.Id.Equals(userId))
                .Include(i => i.Authors)
                .FirstOrDefaultAsync();

            if (response == null)
            {
                _logger.LogError($"Error: An unexpected error occurred at trying to get user with ID: {userId.ToString()}.");
                throw new RestException(HttpStatusCode.NotFound, "The provided identifier does not match any user.");
            }
            return response;
        }

        public async Task<UserDTO> GetById(string userId)
        {
            var response = await _dbcontext.Users
                .Where(w => w.Id.Equals(Guid.Parse(userId)))
                .Select(s => new UserDTO
                {
                    Id = s.Id,
                    Name = s.Name,                    
                    CreatedAt = s.CreatedAt,
                    Email = s.Email,
                    ImageURL = s.ImageURL,
                    Authors = s.Authors.Select(q=> new AuthorDTO { 
                        Name = q.Name,
                        DateOfBirth = q.DateOfBirth,
                        Nationality = q.Nationality
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (response == null)
            {
                _logger.LogError($"Error: An unexpected error occurred at trying to get the user with ID: {userId}.");
                throw new RestException(HttpStatusCode.NotFound, 
                    $"The provided identifier: {userId} does not match any user.");
            }

            return _mapper.Map<UserDTO>(response);
        }

        public async Task<bool> DeleteAsync(Guid userId)
        {
            try
            {
                var entity = GetById(userId).Result;
                _dbcontext.Entry<User>(entity).State = EntityState.Deleted;
                await _dbcontext.SaveChangesAsync();
                return true;
            }
            catch (System.Exception e) 
            {                
                _logger.LogError(e.Message);
                throw new RestException(HttpStatusCode.InternalServerError,
                    "An unexpected error occurred while trying to perform the operation: Delete User.");
            }
        }

        public async Task<User> UpdateImage(Guid userId, UserImage userImage)
        {
            try
            {
                var user = GetById(userId).Result;
                user.ImageURL = userImage.Url;
                var result = _dbcontext.Users.Update(user).Entity;
                await _dbcontext.SaveChangesAsync();
                return result;
            }
            catch (System.Exception e)
            {
                _logger.LogError(e.Message);
                throw new RestException(HttpStatusCode.InternalServerError,
                    "An unexpected error occurred while trying to perform the operation: Update User Image.");
            }            
        }

        public async Task<Object> GetAllUsers(UserParameters queryParameters)
        {
            try
            {
                var query = _dbcontext.Users.Select(s => new UserListDTO
                {
                    Id = s.Id,
                    Name = s.Name,
                    Email = s.Email,
                    ImageURL = s.ImageURL,
                    CreatedAt = s.CreatedAt,
                    AuthorsAmount = s.Authors.Count
                });

                var usersResult = await PagedList<UserListDTO>.ToPagedList(query,
                   queryParameters.Offset,
                   queryParameters.Limit);

                return new
                {
                    usersResult.TotalCount,
                    usersResult.PageSize,
                    usersResult.CurrentPage,
                    usersResult.TotalPages,
                    usersResult.HasNext,
                    usersResult.HasPrevious,
                    rows = usersResult
                };
            }
            catch (System.Exception e) 
            {
                _logger.LogError(e.Message);
                throw new RestException(HttpStatusCode.InternalServerError,
                    "An unexpected error occurred while trying to perform the operation: Get All Users.");
            }
        }

        public async Task<bool> SubscribeToAuthor(Guid userId, int authorId)
        {

            var user = GetUserById(userId).Result;

            if (user.Authors.FirstOrDefault(f => f.Id.Equals(authorId)) != null)
            {
                throw new RestException(HttpStatusCode.Found,
                    "You are already subscribed to this author.");
            }

            var author = await _dbcontext.Authors.FindAsync(authorId);
            if (author == null)
            {
                throw new RestException(HttpStatusCode.NotFound,
                    $"The provided identifier: {authorId}, does not match any author.");
            }

            user.Authors.Add(author);
            try
            {
                await _dbcontext.SaveChangesAsync();
            }
            catch (System.Exception e)
            {
                _logger.LogError(e.Message);
                throw new RestException(HttpStatusCode.InternalServerError,
                    "An unexpected error occurred while trying to perform the operation: Subscribe To Author");
            }

            return true;
        }

        public async Task<bool> DeleteSubscriptionToAuthor(Guid userId, int authorId)
        {
            var user = GetUserById(userId).Result;

            var author = user.Authors.FirstOrDefault(f => f.Id.Equals(authorId));

            if (author == null)
            {
                throw new RestException(HttpStatusCode.NotFound,
                    $"You are not subscribed to this author. Author ID: {authorId}.");
            }
            else
            {
                try
                {
                    user.Authors.Remove(author);
                    await _dbcontext.SaveChangesAsync();
                }
                catch (System.Exception e)
                {
                    _logger.LogError(e.Message);
                    throw new RestException(HttpStatusCode.InternalServerError,
                        "An unexpected error occurred while trying to perform the operation: Delete Subscription To Author");
                }
            }

            return true;
        }

        public bool VerifyUsername(string username) 
        {
            return _dbcontext.Users.Select(s => s.Name.Equals(username)).FirstOrDefault();            
        }
    }
}
