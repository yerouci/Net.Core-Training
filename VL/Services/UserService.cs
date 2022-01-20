using AutoMapper;
using Entities;
using Entities.Models;
using Entities.ModelsDTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VL.Contracts;

namespace VL.Services
{
    public class UserService : IUserService
    {
        protected VLDBContext _dbcontext { get; set; }
        private readonly IMapper _mapper;

        public UserService(VLDBContext dbcontext, IMapper mapper)
        {
            _dbcontext = dbcontext;
            _mapper = mapper;
        }

        public async Task<User> Add(UserInputDTO input)
        {
            User item = _mapper.Map<User>(input);

            item.Id = Guid.NewGuid();
            item.CreatedAt = DateTime.UtcNow;

            var entity = await _dbcontext.Users.AddAsync(item);
            _dbcontext.SaveChanges();

            return entity.Entity;
        }

        public async Task<User> GetById(Guid userId)
        {
            return await _dbcontext.Users.FindAsync(userId);
        }

        public async Task<bool> DeleteAsync(Guid userId)
        {
            var entity = GetById(userId).Result;
            _dbcontext.Entry<User>(entity).State = EntityState.Deleted;
            await _dbcontext.SaveChangesAsync();

            return true;
        }

        public async Task<User> UpdateImage(Guid userId, UserImage userImage)
        {
            var user = GetById(userId).Result;
            user.ImageURL = userImage.Url;
            var result = _dbcontext.Users.Update(user).Entity;
            await _dbcontext.SaveChangesAsync();
            return result;
        }

        public async Task<Object> GetAllUsers(UserParameters queryParameters)
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

        public async Task<bool> SubscribeToAuthor(Guid userId, int authorId) 
        {
            var user = GetById(userId).Result;
            var author = _dbcontext.Authors.Find(authorId);
            if (user.Authors == null)
            {
                user.Authors = new List<Author>();
                user.Authors.Add(author);
            }
            else {
                user.Authors.Add(author);
            }
            await _dbcontext.SaveChangesAsync();
            return true;
        }


        public bool VerifyUsername(string username) 
        {
            return _dbcontext.Users.Select(s => s.Name.Equals(username)).FirstOrDefault();            
        }
    }
}
