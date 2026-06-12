namespace RetailECommerce.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RetailECommerce.Models;
using RetailECommerce.Services.Factory;
using RetailECommerce.Services.Repository;
using RetailECommerce.Services.State.Enquiry;
using System.Security.Claims;


public class EnquiryController : Controller
{

    
    private IEnquiryRepository _enquiryRepository;
    private readonly MyDbContext _context;

    public EnquiryController(IEnquiryRepository enquiryRepository, MyDbContext context)
    {
        _enquiryRepository = enquiryRepository;
        _context = context;
    }

    private int GetCurrentUserId()
        {
            var email = User.FindFirstValue(ClaimTypes.Name)
                        ?? HttpContext.Session.GetString("UserEmail");

            if (!string.IsNullOrEmpty(email))
            {
                var user = _context.Users.FirstOrDefault(u => u.Email == email);

                if (user != null)
                {
                    return user.UserId;
                }
            }

            return 1;
        }

    public IActionResult Index()
    {
        var enquiries = _enquiryRepository.GetAllEnquiries();
        ViewBag.Enquiries = enquiries;
        PageCreator pageCreator = new EnquiriesPageCreator();
        return pageCreator.RenderPage(this);
     }


     [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult SubmitEnquiry(int ProductId, string Message)
    {
        if (string.IsNullOrWhiteSpace(Message))
        {
            TempData["EnquiryError"] = "Please enter your question.";
            return RedirectToAction("Details", "Products", new { id = ProductId, tab = "questions" });
        }

        var enquiry = new Enquiry
        {
            ProductId = ProductId,
            UserId = GetCurrentUserId(),
            Message = Message,
            ReplyMessage = "",
            Status = "Pending",
            CreatedAt = DateTime.Now
        };

        _enquiryRepository.AddEnquiry(enquiry);

        TempData["EnquiryMessage"] = "Question submitted!";
        return RedirectToAction("Details", "Products", new { id = ProductId, tab = "questions" });
    }



     [HttpPost]
     // this action method for submiting a reply to an enquiry,
     // it receives the enquiry object with the updated reply message from the form submission in the view
     public IActionResult UpdateEnquiry(Enquiry enquiry)
     {
         var existingEnquiry = _enquiryRepository.GetEnquiryById(enquiry.EnquiryId);

         if (existingEnquiry == null)
         {
             return NotFound();
         }

         try
        {
            // 5. CLIENT USAGE ( STATE MANAGER USAGE )
            // create the state manager with the existing enquiry and submit the response through the state manager
            var stateManager = new EnquiryStateManager(existingEnquiry);

            // submit the response through the state manager which will handle the state transition 
            // and update the enquiry status accordingly
            stateManager.SubmitResponse(enquiry.ReplyMessage);

             // update the enquiry in the repository with the new reply message and status
            _enquiryRepository.UpdateEnquiry(existingEnquiry);

            TempData["SuccessMessage"] = $"Enquiry with ID {enquiry.EnquiryId} updated successfully.";

            return RedirectToAction("Index");
        } catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Error updating enquiry: {ex.Message}";
            return RedirectToAction("Index");
        }
     }



     [HttpPost]
     public IActionResult VendorUpdateEnquiry(Enquiry enquiry)
     {
         var existingEnquiry = _enquiryRepository.GetEnquiryById(enquiry.EnquiryId);

         if (existingEnquiry == null)
        {
            return NotFound();
        }

        try
        {
            // 5. CLIENT USAGE ( STATE MANAGER USAGE )
            // create the state manager with the existing enquiry and submit the response through the state manager
            var stateManager = new EnquiryStateManager(existingEnquiry);

            // submit the response through the state manager which will handle the state transition 
            // and update the enquiry status accordingly
            stateManager.VendorSubmitResponse(enquiry.ReplyMessage);

             // update the enquiry in the repository with the new reply message and status
            _enquiryRepository.VendorUpdateEnquiry(existingEnquiry);

            _context.Notifications.Add(new Notification
            {
                UserId = existingEnquiry.UserId,
                Message = $"Vendor replied to your enquiry for Product #{existingEnquiry.ProductId}.",
                Type = NotificationType.SystemAlert,
                CreatedAt = DateTime.UtcNow
            });

            _context.SaveChanges();

            TempData["SuccessMessage"] = $"Enquiry with ID {enquiry.EnquiryId} updated successfully.";

            return RedirectToAction("Details", "Products", new { id = existingEnquiry.ProductId, tab = "questions" });

        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Error updating enquiry: {ex.Message}";
            return RedirectToAction("Details", "Products", new { id = existingEnquiry.ProductId, tab = "questions" });
        }

        }

     
      [HttpPost]
      public IActionResult DeleteEnquiryReply(int enquiryId)
    {
        var existingEnquiry = _enquiryRepository.GetEnquiryById(enquiryId);

        if (existingEnquiry == null)
        {
            return NotFound();
        }

        try
        {
            _enquiryRepository.DeleteEnquiryReply(enquiryId);

            TempData["SuccessMessage"] = $"Enquiry reply with ID {enquiryId} deleted successfully.";

            return RedirectToAction("Details", "Products", new { id = existingEnquiry.ProductId, tab = "questions" });
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Error deleting enquiry reply: {ex.Message}";
            return RedirectToAction("Details", "Products", new { id = existingEnquiry.ProductId, tab = "questions" });
        }
    }




     [HttpPost]
     public IActionResult CloseEnquiry(int enquiryId)
    {
        var existingEnquiry = _enquiryRepository.GetEnquiryById(enquiryId);

        if (existingEnquiry == null)
        {
            return NotFound();
        }


        try
        {
           var stateManager = new EnquiryStateManager(existingEnquiry);


           stateManager.CloseEnquiry();

            _enquiryRepository.UpdateEnquiry(existingEnquiry);

            TempData["SuccessMessage"] = $"Enquiry with ID {enquiryId} closed successfully.";

            return RedirectToAction("Index"); 
        } 
        
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Error closing enquiry: {ex.Message}";
            return RedirectToAction("Index");
        }
    }


   





}