using MailSender.Core.Models;
using System;

namespace MailSender.Example.Models
{
    public class WelcomeModel : BaseModel
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public string WWWRootPath { get; set; }

        public override string ToBase64(string path)
        {
            if (string.IsNullOrEmpty(WWWRootPath))
                throw new ArgumentNullException(nameof(WWWRootPath));

            return base.ToBase64(WWWRootPath + path);
        }
    }
}