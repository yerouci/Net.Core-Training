using AutoMapper;
using Entities;
using Entities.Models;
using Entities.ModelsDTO;
using System.Threading.Tasks;
using VL.Contracts;

namespace VL.Services
{
    public class AuthorService : IAuthorService
    {

        protected VLDBContext _dbcontext { get; set; }
        private readonly IMapper _mapper;

        public AuthorService(VLDBContext dbcontext, IMapper mapper)
        {
            _dbcontext = dbcontext;
            _mapper = mapper;
        }

        public async Task<PagedList<Author>> GetAuthors(AuthorParameters queryParameters)
        {
            return await PagedList<Author>.ToPagedList(_dbcontext.Authors,
                queryParameters.Offset,
                queryParameters.Limit);
        }

        public async Task<Author> AddAuthor(AuthorInputDTO input) 
        {
            var author = _mapper.Map<Author>(input);

            var entity = await _dbcontext.Authors.AddAsync(author);
            _dbcontext.SaveChanges();

            return entity.Entity;
        }

        public async Task<AuthorDetailsDTO> GetAuthorDetails(int id) 
        {
            var author = await _dbcontext.Authors.FindAsync(id);

            var details = _mapper.Map<AuthorDetailsDTO>(author);

            var books = _dbcontext.Books;

            return details;
        }

        public async Task<bool> AddNewBook(int id, BookInputDTO input)
        {
            var book = _mapper.Map<Book>(input);
            var author = _dbcontext.Authors.Find(id);

            book.Author = author;

            var result = await _dbcontext.Books.AddAsync(book);
            _dbcontext.SaveChanges();

            return true;
        }
    }
}
