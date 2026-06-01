namespace RetailECommerce.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RetailECommerce.Models;
using RetailECommerce.Services.Factory;
using RetailECommerce.Services.Repository;


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
         _enquiryRepository.UpdateEnquiry(enquiry);
         return RedirectToAction("Index");
     }





}