/*
MIT License

Copyright (c) 2020 trytocatchme

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace MailSender.Core.EmailClients
{
    public class MailClient : IMailClient
    {
        private readonly SmtpConfiguration _smtpConfiguration;

        public MailClient(SmtpConfiguration smtpConfiguration)
        {
            if (smtpConfiguration == null)
                throw new ArgumentNullException(nameof(smtpConfiguration));

            _smtpConfiguration = smtpConfiguration;
        }

        public async Task SendEmailAsync(MailMessage mailMessage)
        {
            if (mailMessage == null)
                throw new ArgumentNullException(nameof(mailMessage));

            try
            {
                using (var smtpClient = new SmtpClient(_smtpConfiguration.Host, _smtpConfiguration.Port))
                {
                    if (!string.IsNullOrEmpty(_smtpConfiguration.Username) && !string.IsNullOrEmpty(_smtpConfiguration.Password))
                        smtpClient.Credentials = new NetworkCredential(_smtpConfiguration.Username, _smtpConfiguration.Password);
                    
                    await smtpClient.SendMailAsync(mailMessage);
                }
            }
            finally
            {
                mailMessage.Dispose();
            }
        }

        public async Task SendEmailAsync(string from, string to, string subject, string body)
        {
            if (string.IsNullOrEmpty(from))
                throw new ArgumentNullException(nameof(from));
            if (string.IsNullOrEmpty(to))
                throw new ArgumentNullException(nameof(to));
            if (string.IsNullOrEmpty(subject))
                throw new ArgumentNullException(nameof(subject));
            if (string.IsNullOrEmpty(body))
                throw new ArgumentNullException(nameof(body));

            var message = new MailMessage();
            message.From = new MailAddress(from);
            message.To.Add(to);
            message.Subject = subject;
            message.Body = body;
      
            await SendEmailAsync(message);
        }
    }
}