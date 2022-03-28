using System.Collections.Generic;
using System.Linq;

namespace BookAPIProject.Models.Services.Classes
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly BookDbContext _reviewContext;
        public ReviewRepository(BookDbContext reviewContext)
        {
            _reviewContext = reviewContext;
        }
        public Book GetBookOfReview(int ReviewId)
        {
            var bookId = _reviewContext.Reviews.Where(r=>r.Id == ReviewId).Select(
                b=>b.Book.Id).FirstOrDefault();
            return _reviewContext.Books.Where(b=>b.Id== bookId).FirstOrDefault();
        }

        public Review GetReview(int ReviewId)
        {
            return _reviewContext.Reviews.Where(r=>r.Id == ReviewId).FirstOrDefault();
        }

        public ICollection<Review> GetReviews()
        {
            return _reviewContext.Reviews.OrderBy(r=>r.Rating).ToList();
        }

        public ICollection<Review> GetReviewsOfBook(int BookId)
        {
            return _reviewContext.Reviews.Where(r => r.Book.Id == BookId).ToList();
        }

        public bool ReviewExists(int ReviewId)
        {
            return _reviewContext.Reviewers.Any(r => r.Id == ReviewId);
        }

        public bool Add(Review review)
        {
            _reviewContext.Add(review);
            return Save();
        }

        public bool Update(Review review)
        {
            _reviewContext.Update(review);
            return Save();
        }

        public bool Delete(Review review)
        {
            _reviewContext.Remove(review);
            return Save();
        }

        public bool Save()
        {
            var saved = _reviewContext.SaveChanges();
            return saved >= 0 ? true : false;
        }
    }
}
