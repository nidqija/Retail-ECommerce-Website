namespace RetailECommerce.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RetailECommerce.Services.Factory;
using RetailECommerce.Services.Repository;
using RetailECommerce.Models;





public class ReviewController : Controller
{

    private readonly IReviewRepository _reviewRepository;
    private readonly MyDbContext _context;


    public ReviewController(IReviewRepository reviewRepository,MyDbContext context)
    {
        _reviewRepository = reviewRepository;
        _context = context;
    }
    

    public IActionResult Index()
    {


        var reviews  = _reviewRepository.GetAllReviews();
        
        // viewbag is used to pass the reviews data to view 
        // so that it can be displayed in the reviews management page
        ViewBag.Reviews = reviews;
        
        PageCreator creator = new ReviewsPageCreator();
        return creator.RenderPage(this);
    }


    [HttpPost]
    public IActionResult UpdateReview(Review review)
    {
        try
        {
            var existingReview = _reviewRepository.GetReviewById(review.ReviewId);

            if (existingReview == null)
            {
                return NotFound("Review not found");
            }


            existingReview.VendorReply = review.VendorReply;
            existingReview.Status = "Replied";

            _reviewRepository.UpdateReview(existingReview);

            _context.Notifications.Add(new Notification
            {
                UserId = existingReview.UserId,
                Message = $"Vendor replied to your feedback for Product #{existingReview.ProductId}.",
                Type = NotificationType.SystemAlert,
                ProductId = existingReview.ProductId,
                Tab = "feedback",
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            });

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        catch (Exception ex)
        {
            // Log the exception (not implemented here)
            return StatusCode(500, "An error occurred while updating the review: " + ex.Message);
        }
    }


    [HttpPost]
    public IActionResult DeleteVendorReply(int reviewId , int productId)
    {
       try
        {
            _reviewRepository.DeleteVendorReply(reviewId);


            if (productId > 0)
                {
                    return RedirectToAction("Details", "Products", new { id = productId });
                }
                
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            // Log the exception (not implemented here)
            return StatusCode(500, "An error occurred while deleting the vendor reply: " + ex.Message);
        }
    }

    


    
    
    
}