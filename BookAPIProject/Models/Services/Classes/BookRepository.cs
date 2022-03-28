using System.Collections.Generic;
using System.Linq;

namespace BookAPIProject.Models.Services.Classes
{
    public class BookRepository : IBookRepository
    {
        private readonly BookDbContext _bookdbContext;
        public BookRepository(BookDbContext bookdbContext)
        {
            _bookdbContext = bookdbContext;
        }
        public bool BookExists(int BookId)
        {
            return _bookdbContext.Books.Any(b=>b.Id == BookId);
        }

        public bool BookExists(string BookIsbn)
        {
            return _bookdbContext.Books.Any(b => b.Isbn == BookIsbn);
        }

        public bool BookExists(int BookId, string BookIsbn)
        {
            throw new System.NotImplementedException();
        }

        public Book GetBook(int BookId)
        {
            return _bookdbContext.Books.Where(b=>b.Id == BookId).FirstOrDefault();
        }

        public Book GetBook(string BookIsbn)
        {
            return _bookdbContext.Books.Where(b => b.Isbn == BookIsbn).FirstOrDefault();
        }

        public decimal GetBookRating(int BookId)
        {
            var review = _bookdbContext.Reviews.Where(r => r.Id == BookId);
            if(!review.Any())
            {
                return  0;
            }
            return review.Sum(r => r.Rating) / review.Count();
        }

        public ICollection<Book> GetBooks()
        {
            return _bookdbContext.Books.OrderBy(b=>b.Title).ToList();
        }

        public bool IDuplicateIsbn(int BookId, string BookIsbn)
        {
            var book = _bookdbContext.Books.Where(b=>b.Isbn.Trim().ToUpper() == BookIsbn.Trim().ToUpper() && b.Id!=BookId).FirstOrDefault();
            return book == null ? false : true;
        }
    }
}
