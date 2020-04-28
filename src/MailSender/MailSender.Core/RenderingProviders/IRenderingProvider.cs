using Microsoft.AspNetCore.Mvc.ViewEngines;
using System.Threading.Tasks;

namespace MailSender.Core.RenderingProviders
{
    public interface IRenderingProvider
    {
        Task<string> RenderAsync(IView view, object model);
    }
}