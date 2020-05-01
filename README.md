# MailSender
MailSender is a pure .net core library written in C#. Used for sending complex emails.

**How it works?**
----------------
![MailSenderWorkflow](https://github.com/trytocatchme/MailSender/blob/master/MailSenderWorkflow.png)
<br />
<br />

**How to use**
<br />
Example of implementation you can find in <b>MailSender.Example</b> project in <b>ReportController.cs</b>

1. Download MailSender from nuget: https://www.nuget.org/packages/MailSender.NetCore/
2. Add Smtp configuration to <b>appsettings.json</b>
```
"SMTP": {
  "Host": "",
  "Port": 25,
  "Username": "",
  "Password": ""
},
```
3.Add below code to <b>Startup.cs</b> in <b>public void ConfigureServices(IServiceCollection services)</b> method.
```
services.AddSingleton((x) =>
{
    var smtpConfiguration = new SmtpConfiguration();
    Configuration.GetSection("Smtp").Bind(smtpConfiguration);
    return smtpConfiguration;
});
services.AddScoped<MailSenderClient>();
```
4. Start local Smtp server (it's not a part of this library) for example this one: https://github.com/ChangemakerStudios/Papercut-SMTP
5. Put Smtp server configuration to <b>appsettings.json</b> 

```
Example configuration:
"SMTP": {
  "Host": "127.0.0.1",
  "Port": 25,
  "Username": "",
  "Password": ""
},
```
6. Create ReportController in ASP.NET Core application and add below code:
```
   public class ReportController : Controller// you can name it as you want
    {
        private readonly IWebHostEnvironment _webHostEnvironment; //for simple Email you don't need to do this but if you want to send Email with local images(assets included in solution), you have to inject this.
        private readonly MailSenderClient _mailSenderClient;

        public ReportController(IWebHostEnvironment webHostEnvironment, MailSenderClient mailSenderClient)
        {
            _webHostEnvironment = webHostEnvironment;
            _mailSenderClient = mailSenderClient;
        }
        
        ...
```
7. Create WelcomeModel. It will be used for passing data to razor view.
```
    public class WelcomeModel : BaseModel
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public string WWWRootPath { get; set; }

        public override string ToBase64(string path)// it's helper method that we will use for transforming images to base64 format
        {
            if (string.IsNullOrEmpty(WWWRootPath))
                throw new ArgumentNullException(nameof(WWWRootPath));

            return base.ToBase64(WWWRootPath + path);
        }
    }
```
8. In ASP.NET core application add logo.jpg(pick random image from internet) to wwwroot\assets\logo.jpg
9. Create view called Welcome.cshtml (Views/Report/Welcome.cshtml) and paste below code:
```
@model MailSender.Example.Models.WelcomeModel

<h1>@Model.Title</h1>
<p>@Model.Body</p>

<img height="50" src="@Model.ToBase64(@"\assets\logo.jpg")" />
```
10. In the controller that you created previousrly past this code:
```
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
```
11. If you configured everything corectly you should see output from the action in your browser and also receive an email.
<br />
-Test links: https://localhost:44337/report/welcome or https://localhost:44337/report/Complex
<br />
*(Probably port on your machine will be different. Remember also to setup MailSender.Example as a startup project).



