using BookAPIProject.Controllres.Dtos;
using BookAPIProject.Models.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace BookAPIProject.Controllres
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : Controller
    {
        private readonly IAuthorRepository _authorRepository;
        public AuthorController(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        [HttpGet]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<AuthorDto>))]
        public IActionResult GetAuthors()
        {
            var Authors = _authorRepository.GetAuthors();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var authorDto = new List<AuthorDto>();
            foreach (var author in Authors)
            {
                authorDto.Add(new AuthorDto
                {
                    Id = author.Id,
                    FirstName = author.FirstName,
                    LastName = author.LastName,
                });
            }
            return Ok(authorDto);
        }

        [HttpGet("{authorId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(AuthorDto))]
        public IActionResult GetAuthor(int authorId)
        {
            if (!_authorRepository.AuthorExists(authorId))
            {
                return NotFound();
            }
            var authors = _authorRepository.GetAuthor(authorId);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var authorDto = new AuthorDto()
            {
                Id = authors.Id,
                FirstName = authors.FirstName,
                LastName = authors.LastName,
            };

            return Ok(authorDto);
        }

        [HttpGet("books/{bookId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<AuthorDto>))]
        public IActionResult GetAuthorsOfBook(int bookId)
        {
            
            var authors = _authorRepository.GetAuthorsOfBook(bookId);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var authorDto = new List<AuthorDto>();
            foreach (var author in authors)
            {
                authorDto.Add(new AuthorDto()
                {
                    Id = author.Id,
                    FirstName = author.FirstName,
                    LastName = author.LastName,
                });
            }
            return Ok(authorDto);
        }


        [HttpGet("{authorId}/books")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(BookDto))]
        public IActionResult GetBooksByAuthor(int authorId)
        {
            if (!_authorRepository.AuthorExists(authorId))
            {
                return NotFound();
            }
            var books = _authorRepository.GetBooksByAuthor(authorId);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var booksDto = new List<BookDto>();
            foreach (var book in books)
            {
                booksDto.Add(new BookDto
                {
                    Id = book.Id,
                    Title = book.Title,
                    Isbn = book.Isbn,
                    DatePublished = book.DatePublished
                });
            }
            return Ok(booksDto);
        }
    }
}
