using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace MailSender.Core.ViewPickers
{
    public interface IViewPicker
    {
        IView GetView(string viewName);
    }
}