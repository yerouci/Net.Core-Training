using Entities;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VL.Contracts;

namespace VL.Services
{
    public class AuthorService : IAuthorService
    {

        protected VLDBContext dbcontext { get; set; }

        public AuthorService(VLDBContext _dbcontext)
        {
            dbcontext = _dbcontext;
        }

        public async Task<PagedList<Author>> GetAuthors(AuthorParameters queryParameters)
        {
            return await PagedList<Author>.ToPagedList(dbcontext.Authors,
                queryParameters.Offset,
                queryParameters.Limit);
        }
    }
}
