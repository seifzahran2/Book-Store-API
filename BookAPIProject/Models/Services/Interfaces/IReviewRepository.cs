using System.Collections.Generic;

namespace BookAPIProject.Models.Services
{
    public interface IReviewRepository
    {
        ICollection<Review> GetReviews();
        Review GetReview(int ReviewId);
        ICollection<Review> GetReviewsOfBook(int BookId);
        Book GetBookOfReview(int ReviewId);
        bool ReviewExists(int ReviewId);
        bool Add(Review review);
        bool Delete(Review review);
        bool Update(Review review);
        bool Save();
    }
}
