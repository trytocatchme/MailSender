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

        public async Task<IActionResult> Welcome()
        {
            MailMessage mailMessage = new MailMessage("sender@user.com", "receiver@user.com")
            {
                IsBodyHtml = true,
                Subject = "Email subject"
            };

            var dataModel = new WelcomeModel()
            {
                Title = "Welcome everyone",
                Body = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                WWWRootPath = _webHostEnvironment.WebRootPath
            };

            var mailModel = new MailModel(mailMessage, dataModel, "Report/Welcome");
            await _mailSenderClient.SendAsync(mailModel);

            return View(dataModel);
        }

        public async Task<IActionResult> Complex()
        {
            MailMessage mailMessage = new MailMessage("sender@user.com", "receiver@user.com")
            {
                IsBodyHtml = true,
                Subject = "Complex email"
            };

            var dataModel = new WelcomeModel()
            {
                Title = "Welcome everyone",
                Body = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                WWWRootPath = _webHostEnvironment.WebRootPath
            };

            var mailModel = new MailModel(mailMessage, dataModel, "Report/Complex");
            await _mailSenderClient.SendAsync(mailModel);

            return View(dataModel);
        }
    }
}