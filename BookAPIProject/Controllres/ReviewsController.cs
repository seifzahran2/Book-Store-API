using BookAPIProject.Controllres.Dtos;
using BookAPIProject.Models;
using BookAPIProject.Models.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace BookAPIProject.Controllres
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : Controller
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IReviewerRepository _reviewerRepository;
        private readonly IBookRepository _bookRepository;
        public ReviewsController(IReviewRepository reviewRepository, IBookRepository bookRepository, IReviewerRepository reviewerRepository)
        {
            _reviewRepository = reviewRepository;
            _bookRepository = bookRepository;
            _reviewerRepository = reviewerRepository;   
        }
        [HttpGet]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewDto>))]
        public IActionResult GetReviews()
        {
            var reviews = _reviewRepository.GetReviews().ToList();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var reviewsDto = new List<ReviewDto>();
            foreach (var review in reviews)
            {
                reviewsDto.Add(new ReviewDto
                {
                    Id = review.Id,
                    HeadLine = review.HeadLine,
                    ReviewText = review.ReviewText,
                    Rating = review.Rating,
                });
            }
            return Ok(reviewsDto);
        }

        [HttpGet("{reviewId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(ReviewDto))]
        public IActionResult GetReview(int reviewId)
        {
            if (!_reviewRepository.ReviewExists(reviewId))
            {
                return NotFound();
            }
            var review = _reviewRepository.GetReview(reviewId);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var reviewDto = new ReviewDto()
            {
                Id = review.Id,
                HeadLine = review.HeadLine,
                ReviewText = review.ReviewText,
                Rating = review.Rating,
            };

            return Ok(reviewDto);
        }

        [HttpGet("Book/{bookId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewDto>))]
        public IActionResult GetReviewsOfBook(int bookId)
        {
            if (!_bookRepository.BookExists(bookId))
            {
                return NotFound();
            }
            var reviews = _reviewRepository.GetReviewsOfBook(bookId);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var ReviewDto = new List<ReviewDto>();
            foreach (var review in reviews)
            {
                ReviewDto.Add(new ReviewDto()
                {
                    Id = review.Id,
                    HeadLine = review.HeadLine,
                    ReviewText = review.ReviewText,
                    Rating = review.Rating,
                });
            }
            return Ok(ReviewDto);
        }

        [HttpGet("{reviewId}/books")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(BookDto))]
        public IActionResult GetBookOfReview(int reviewId)
        {
            if (!_reviewRepository.ReviewExists(reviewId))
            {
                return NotFound();
            }
            var book = _reviewRepository.GetBookOfReview(reviewId);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var booksDto = new BookDto() { 
                    Id = book.Id,
                    Title = book.Title,
                    Isbn = book.Isbn,
                    DatePublished = book.DatePublished
                };
            
            return Ok(booksDto);
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Review))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult CreateReview([FromBody] Review Reviewcreate)
        {
            if (Reviewcreate == null)
                return BadRequest(ModelState);

            if (!_reviewerRepository.ReviewerExists(Reviewcreate.Reviewer.Id))
                 ModelState.AddModelError("","this reviewer not found");

            if (!_bookRepository.BookExists(Reviewcreate.Book.Id))
                ModelState.AddModelError("", "this book not found");

            if (!ModelState.IsValid)
                return StatusCode(404, ModelState);

            Reviewcreate.Book = _bookRepository.GetBook(Reviewcreate.Book.Id);
            Reviewcreate.Reviewer = _reviewerRepository.GetReviewer(Reviewcreate.Reviewer.Id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if(_reviewRepository.Add(Reviewcreate))
            {
                ModelState.AddModelError("",$"Some thing went wrong saving the review");
                return StatusCode(500, ModelState);
            }
            
            return CreatedAtRoute("GetReview", new { ReviewId = Reviewcreate.Id }, Reviewcreate);
        }

        
    }
}
