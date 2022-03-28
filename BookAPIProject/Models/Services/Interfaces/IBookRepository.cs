using System.Collections;
using System.Collections.Generic;

namespace BookAPIProject.Models.Services
{
    public interface IBookRepository
    {
        ICollection<Book> GetBooks();
        Book GetBook(int BookId);
        Book GetBook(string BookIsbn);
        decimal GetBookRating(int BookId);
        bool BookExists (int BookId);
        bool BookExists(string BookIsbn);
        bool BookExists(int BookId, string BookIsbn);
        bool IDuplicateIsbn(int BookId, string BookIsbn);
    }
}
