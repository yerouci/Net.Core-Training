using Entities;
using Entities.Models;
using Entities.ModelsDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VL.Contracts
{
    public interface IAuthorService
    {
        Task<PagedList<Author>> GetAuthors(AuthorParameters bookParameters);
        Task<Author> AddAuthor(AuthorInputDTO input);
        Task<AuthorDetailsDTO> GetAuthorDetails(int id);
        Task<BookDTO> AddNewBook(int id, BookInputDTO input);
    }
}
