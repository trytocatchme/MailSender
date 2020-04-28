using System;
using System.Net.Mail;
using System.Threading.Tasks;
using MailSender.Core;
using MailSender.Core.Models;
using MailSender.Core.RenderingProviders;
using MailSender.Core.ViewPickers;
using MailSender.Example.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace MailSender.Example.Controllers
{
    public class ReportController : Controller
    {
        private readonly IRazorViewEngine _razorViewEngine;
        private readonly ITempDataProvider _tempDataProvider;
        private readonly IServiceProvider _serviceProvider;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly SmtpConfiguration _smtpConfiguration;
        private readonly MailSenderClient _mailSenderClient;


        public ReportController(IRazorViewEngine razorViewEngine,
            ITempDataProvider tempDataProvider,
            IServiceProvider serviceProvider,
            IWebHostEnvironment webHostEnvironment,
            SmtpConfiguration smtpConfiguration)
        {
            _razorViewEngine = razorViewEngine;
            _tempDataProvider = tempDataProvider;
            _serviceProvider = serviceProvider;
            _webHostEnvironment = webHostEnvironment;
            _smtpConfiguration = smtpConfiguration;

            _mailSenderClient = new MailSenderClient(razorViewEngine, tempDataProvider, serviceProvider, smtpConfiguration);

        }

        public async Task<IActionResult> Index()
        {
            MailMessage mailMessage = new MailMessage("Papercut@user.com", "Papercut@user.com")
            {
                IsBodyHtml = true
            };

            var dataModel = new WelcomeModel()
            {
                Name = "Artur",
                Title = "Tytul",
                WWWRootPath = _webHostEnvironment.WebRootPath
            };

            var mailModel = new MailModel(mailMessage, dataModel, "Report/Welcome");

            await _mailSenderClient.SendAsync(mailModel);

          

           
            return View("Welcome", dataModel);
        }
    }
}