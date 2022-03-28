using System.Collections.Generic;

namespace BookAPIProject.Models.Services
{
    public interface IReviewerRepository
    {
        ICollection<Reviewer> GetReviewers();
        Reviewer GetReviewer(int ReviewerId);
        Reviewer GetReviewerOfReview(int ReviewId);
        ICollection<Review> GetReviewsByReviewer(int ReviewerId);
        bool ReviewerExists(int ReviewerId);
    }
}
