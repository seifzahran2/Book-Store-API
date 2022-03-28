using System.Collections.Generic;

namespace BookAPIProject.Models.Services
{
    public interface IAuthorRepository
    {
        ICollection<Author> GetAuthors();
        Author GetAuthor(int AuthorId);
        ICollection<Author> GetAuthorsOfBook(int BookId);
        ICollection<Book> GetBooksByAuthor(int AuthorId);
        bool AuthorExists(int AuthorId);
    }
}
