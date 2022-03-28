using System.Collections.Generic;
using System.Linq;

namespace BookAPIProject.Models.Services
{
    public class ReviewerRepository : IReviewerRepository
    {
        private readonly BookDbContext _reviewerContext;
        public ReviewerRepository(BookDbContext reviewerContext)
        {
            _reviewerContext = reviewerContext;
        }
        public Reviewer GetReviewer(int ReviewerId)
        {
            return _reviewerContext.Reviewers.Where(r => r.Id == ReviewerId).FirstOrDefault();
        }

        public Reviewer GetReviewerOfReview(int ReviewId)
        {
             var reviewerId = _reviewerContext.Reviews.Where(r=>r.Id==ReviewId).Select(
                 rr=>rr.Reviewer.Id).FirstOrDefault();
            return _reviewerContext.Reviewers.Where(r => r.Id == reviewerId).FirstOrDefault();
        }

        public ICollection<Reviewer> GetReviewers()
        {
            return _reviewerContext.Reviewers.OrderBy(r => r.FirstName).ToList();
        }

        public ICollection<Review> GetReviewsByReviewer(int ReviewerId)
        {
            return _reviewerContext.Reviews.Where(r => r.Reviewer.Id == ReviewerId).ToList();
        }

        public bool ReviewerExists(int ReviewerId)
        {
            return _reviewerContext.Reviewers.Any(r => r.Id == ReviewerId);
        }
    }
}
