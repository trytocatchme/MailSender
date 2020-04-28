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
using MailSender.Core.EmailClients;
using MailSender.Core.Models;
using MailSender.Core.RenderingProviders;
using MailSender.Core.ViewPickers;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Threading.Tasks;

namespace MailSender.Core
{
    public class MailSenderClient
    {
        private readonly IViewPicker _viewPicker;
        private readonly IRenderingProvider _renderingProvider;
        private readonly IMailClient _mailClient;

        public MailSenderClient(IRazorViewEngine razorViewEngine,
            ITempDataProvider tempDataProvider,
            IServiceProvider serviceProvider,
            SmtpConfiguration smtpConfiguration)
        {
            if (razorViewEngine == null)
                throw new ArgumentNullException(nameof(razorViewEngine));
            
            if (tempDataProvider == null)
                throw new ArgumentNullException(nameof(tempDataProvider));

            if (serviceProvider == null)
                throw new ArgumentNullException(nameof(serviceProvider));

            if (smtpConfiguration == null)
                throw new ArgumentNullException(nameof(smtpConfiguration));

            _viewPicker = new StandardViewPicker(razorViewEngine, serviceProvider);
            _renderingProvider = new RazorRender(tempDataProvider, serviceProvider);
            _mailClient = new MailClient(smtpConfiguration);
        }

        public MailSenderClient(IViewPicker viewPicker, IRenderingProvider renderingProvider, IMailClient mailClient)
        {
            _renderingProvider = renderingProvider ?? throw new ArgumentNullException(nameof(renderingProvider));
            _viewPicker = viewPicker ?? throw new ArgumentNullException(nameof(viewPicker));
            _mailClient = mailClient ?? throw new ArgumentNullException(nameof(mailClient));
        }

        public virtual async Task SendAsync(MailModel mailModel)
        {
            if (mailModel == null)
                throw new ArgumentNullException(nameof(mailModel));

            var view = _viewPicker.GetView(mailModel.ViewName);
            mailModel.MailMessage.Body = await _renderingProvider.RenderAsync(view, mailModel.BaseModel);
            await _mailClient.SendEmailAsync(mailModel.MailMessage);
        }

        public virtual async Task SendAsync(string from, string to, string subject, string body)
        {
            await _mailClient.SendEmailAsync(from, to, subject, body);
        }
    }
}