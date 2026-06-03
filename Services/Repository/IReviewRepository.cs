namespace RetailECommerce.Services.Repository;
using RetailECommerce.Models;



public interface IReviewRepository
{
    IEnumerable<Review> GetAllReviews();

    Review GetReviewById(int reviewId);

    Review GetReviewByUser(int userId);

    Review GetReviewByProduct(int productId);

  
}