using Entities;
using Entities.Models;
using Entities.ModelsDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VL.Contracts
{
    public interface IUserService
    {
        Task<User> Add(UserInputDTO user);
        Task<bool> DeleteAsync(Guid userId);
        Task<User> UpdateImage(Guid userId, UserImage userImage);
        Task<Object> GetAllUsers(UserParameters queryParameters);
        Task<bool> SubscribeToAuthor(Guid userId, int authorId);
        Task<User> GetById(Guid userId);
        Task<UserDTO> GetById(string userId);
        bool VerifyUsername(string username);
        Task<bool> DeleteSubscriptionToAuthor(Guid userId, int authorId);
    }
}
