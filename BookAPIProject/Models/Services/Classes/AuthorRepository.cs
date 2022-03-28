using System.Collections.Generic;
using System.Linq;

namespace BookAPIProject.Models.Services.Classes
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly BookDbContext _authordbContext;
        public AuthorRepository(BookDbContext authordbContext)
        {
            _authordbContext = authordbContext;
        }
        public bool AuthorExists(int AuthorId)
        {
            return _authordbContext.Author.Any(a=>a.Id == AuthorId);
        }

        public Author GetAuthor(int AuthorId)
        {
            return _authordbContext.Author.Where(r=>r.Id == AuthorId).FirstOrDefault();
        }

        public ICollection<Author> GetAuthors()
        {
            return _authordbContext.Author.OrderBy(a=>a.FirstName).ToList();
        }

        public ICollection<Author> GetAuthorsOfBook(int BookId)
        {
            
            return _authordbContext.BookAuthors.Where(b => b.Book.Id == BookId).Select(
                a=>a.Author).ToList();
        }

        public ICollection<Book> GetBooksByAuthor(int AuthorId)
        {
            return _authordbContext.BookAuthors.Where(a => a.Author.Id == AuthorId).Select(
                b => b.Book).ToList();
        }
    }
}
