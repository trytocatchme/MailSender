# MailSender
MailSender is a pure .net core library written in C#. Used for sending complex emails.

**How it works?**
----------------
![MailSenderWorkflow](https://github.com/trytocatchme/MailSender/blob/master/MailSenderWorkflow.png)
<br />
<br />
**Installation**

1. Download MailSender from nuget: https://www.nuget.org/packages/MailSender.NetCore/
2. Add Smtp configuration to <b>appsettings.json</b>
```
"SMTP": {
  "Host": "127.0.0.1",
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

**How to use**
1. Start local Smtp server or use the existing one
2. Put Smtp server configuration to <b>appsettings.json</b>
3. Example of implementation you can find in <b>MailSender.Example</b> project in <b>ReportController.cs</b>
4. If you configured everything corectly you should see output from the action in your browser and also receive an email.
<br />
-Test links: https://localhost:44337/report/welcome or https://localhost:44337/report/Complex
<br />
*(Probably port on your machine will be different. Remember also to setup MailSender.Example as a startup project).



