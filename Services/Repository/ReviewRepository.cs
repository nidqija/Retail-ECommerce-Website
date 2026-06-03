namespace RetailECommerce.Services.Repository;
using Microsoft.EntityFrameworkCore;
using RetailECommerce.Models;




public class ReviewRepository : IReviewRepository
{

    private readonly MyDbContext _context;

    public ReviewRepository(MyDbContext context)
    {
        _context = context;
    }


    public IEnumerable<Review> GetAllReviews()
    {
        return _context.Reviews.Include(r => r.User).Include(r => r.Product).ToList();
    }


    public Review GetReviewById(int reviewId)
    {
        var review = _context.Reviews.Find(reviewId);
        if (review == null)
        {
            throw new Exception("Review not found");
        }
        return review;
    }

    public Review GetReviewByUser(int userId)
    {
        var review = _context.Reviews.FirstOrDefault(r => r.UserId == userId);
        if (review == null)
        {
            throw new Exception("Review not found for the specified user");
        }
        return review;
    }

    public Review GetReviewByProduct(int productId)
    {
        var review = _context.Reviews.FirstOrDefault(r => r.ProductId == productId);
        if (review == null)
        {
            throw new Exception("Review not found for the specified product");
        }
        return review;
    }

    public void UpdateReview(Review review)
    {
        var existingReview = _context.Reviews.Find(review.ReviewId);
        if (existingReview == null)
        {
            throw new Exception("Review not found");
        }

        existingReview.rating = review.rating;
        existingReview.Comment = review.Comment;
        existingReview.status = review.status;
        existingReview.VendorReply = review.VendorReply;
        existingReview.UserId = review.UserId;
        existingReview.ProductId = review.ProductId;

        _context.SaveChanges();
    }


    public void DeleteVendorReply(int reviewId)
    {
        var review = _context.Reviews.Find(reviewId);
        if (review == null)
        {
            throw new Exception("Review not found");
        }

        review.VendorReply = null;
        review.status = "Pending"; 
        _context.SaveChanges();


    }
}