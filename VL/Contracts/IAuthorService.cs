using Entities;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VL.Contracts
{
    public interface IAuthorService
    {
        Task<PagedList<Author>> GetAuthors(QueryStringParameters ownerParameters);
    }
}
