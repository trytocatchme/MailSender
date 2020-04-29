# MailSender
MailSender is a pure .net core library written in C#. Used for sending complex emails.

**How it works?**
----------------
![MailSenderWorkflow](https://github.com/trytocatchme/MailSender/blob/master/MailSenderWorkflow.png)
<br />
<br />
**Installation**

1. Download MailSender from nuget: link
2. Add Smtp configuration to <b>appsettings.json</b>
```
"SMTP": {
  "Host": "127.0.0.1",
  "Port": 25,
  "Username": "",
  "Password": ""
},
```
3.Add below code to <b>Startup.cs</b> in <b>public void ConfigureServices(IServiceCollection services)</b>
```
services.AddSingleton((x) =>
{
    var smtpConfiguration = new SmtpConfiguration();
    Configuration.GetSection("Smtp").Bind(smtpConfiguration);
    return smtpConfiguration;
});
services.AddScoped<MailSenderClient>();

```
