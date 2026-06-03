namespace RetailECommerce.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RetailECommerce.Services.Factory;
using RetailECommerce.Services.Repository;





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
}