namespace RetailECommerce.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RetailECommerce.Models;
using RetailECommerce.Services.Factory;
using RetailECommerce.Services.Repository;
using RetailECommerce.Services.State.Enquiry;


public class EnquiryController : Controller
{

    private IEnquiryRepository _enquiryRepository;

    public EnquiryController(IEnquiryRepository enquiryRepository)
    {
        _enquiryRepository = enquiryRepository;
    }


    public IActionResult Index()
    {
        var enquiries = _enquiryRepository.GetAllEnquiries();
        ViewBag.Enquiries = enquiries;
        PageCreator pageCreator = new EnquiriesPageCreator();
        return pageCreator.RenderPage(this);
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