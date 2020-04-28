using System.Net.Mail;
using System.Threading.Tasks;
using MailSender.Core;
using MailSender.Core.Models;
using MailSender.Example.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace MailSender.Example.Controllers
{
    public class ReportController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly MailSenderClient _mailSenderClient;

        public ReportController(IWebHostEnvironment webHostEnvironment, MailSenderClient mailSenderClient)
        {
            _webHostEnvironment = webHostEnvironment;
            _mailSenderClient = mailSenderClient;
        }

        public async Task<IActionResult> Index()
        {
            MailMessage mailMessage = new MailMessage("sender@user.com", "receiver@user.com")
            {
                IsBodyHtml = true,
                Subject = "Email subject"
            };


            var dataModel = new WelcomeModel()
            {
                Name = "test name",
                Title = "test title",
                WWWRootPath = _webHostEnvironment.WebRootPath
            };

            var mailModel = new MailModel(mailMessage, dataModel, "Report/Welcome");

            await _mailSenderClient.SendAsync(mailModel);

            return View("Welcome", dataModel);
        }
    }
}