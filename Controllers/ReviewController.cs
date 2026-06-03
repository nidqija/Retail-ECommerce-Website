namespace RetailECommerce.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RetailECommerce.Services.Factory;
using RetailECommerce.Services.Repository;
using RetailECommerce.Models;





public class ReviewController : Controller
{

    private readonly IReviewRepository _reviewRepository;


    public ReviewController(IReviewRepository reviewRepository)
    {
        _reviewRepository = reviewRepository;
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
            existingReview.status = "Replied";

            _reviewRepository.UpdateReview(existingReview);
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