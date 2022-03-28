using BookAPIProject.Controllres.Dtos;
using BookAPIProject.Models.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BookAPIProject.Controllres
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : Controller
    {
        private readonly IBookRepository _bookRepository;
        public BookController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }
        [HttpGet]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<BookDto>))]
        public IActionResult GetBooks()
        {
            var books = _bookRepository.GetBooks();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var bookDto = new List<BookDto>();
            foreach (var book in books)
            {
                bookDto.Add(new BookDto
                {
                    Id = book.Id,
                    Isbn = book.Isbn,
                    Title = book.Title,
                    DatePublished = book.DatePublished,
                });
            }
            return Ok(bookDto);
        }

        [HttpGet("{bookId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(BookDto))]
        public IActionResult GetBook(int bookId)
        {
            if (!_bookRepository.BookExists(bookId))
            {
                return NotFound();
            }
            var books = _bookRepository.GetBook(bookId);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var bookDto = new BookDto()
            {
                Id = books.Id,
                Title = books.Title,
                Isbn = books.Isbn,
                DatePublished = books.DatePublished,
            };

            return Ok(bookDto);
        }

        [HttpGet("{bookId}/rating")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(decimal))]
        public IActionResult GetBookRating(int bookId)
        {
            if (!_bookRepository.BookExists(bookId))
            {
                return NotFound();
            }
            var bookRating = _bookRepository.GetBookRating(bookId);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(bookRating);
        }
    }
}
