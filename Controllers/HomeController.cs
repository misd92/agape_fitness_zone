using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Mvc;

public class HomeController : Controller
{
    public HomeController() { }

    public IActionResult Index()
    {
        return View();
    }

    [Route("enquiry")]
    [HttpPost]
    public JsonResult PostEnquiry(ContactFormModel model)
    {
        try
        {
            var fromAddress = "";
            var toAddress = "Sharatshant@Hotmail.Com";
            var fromPassword = "";
            using (MailMessage mm = new MailMessage(new MailAddress(fromAddress), new MailAddress(toAddress)))
            {
                mm.Subject = "Mail from enquiry form";
                mm.Body = $"An enquiry mail received from {model.Name} with email address {model.Email} and mobile number {model.Mobile}";
                mm.IsBodyHtml = false;
                using (var smtp = new SmtpClient())
                {
                    smtp.Host = "smtp.gmail.com";
                    smtp.EnableSsl = true;
                    var NetworkCred = new NetworkCredential(fromAddress, fromPassword);
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = 587;
                    smtp.Send(mm);
                    ViewBag.Message = "Your enquiry is received. We will contact you as soon as possible.";
                }
            }
        }
        catch (Exception ex)
        {
            return new JsonResult("Enquiry submission failed.");
        }
        return new JsonResult("Enquiry sent");
    }
}