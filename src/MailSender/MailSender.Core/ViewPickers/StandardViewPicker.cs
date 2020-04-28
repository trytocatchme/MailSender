using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Routing;
using System;

namespace MailSender.Core.ViewPickers
{
    public class StandardViewPicker : IViewPicker
    {
        private readonly IRazorViewEngine _razorViewEngine;
        private readonly IServiceProvider _serviceProvider;

        public StandardViewPicker(IRazorViewEngine razorViewEngine, IServiceProvider serviceProvider)
        {
            if (razorViewEngine == null)
                throw new ArgumentNullException(nameof(razorViewEngine));

            if (serviceProvider == null)
                throw new ArgumentNullException(nameof(serviceProvider));

            _razorViewEngine = razorViewEngine;
            _serviceProvider = serviceProvider;
        }

        public IView GetView(string viewName)
        {
            if (string.IsNullOrEmpty(viewName))
                throw new ArgumentNullException(nameof(viewName));

            var httpContext = new DefaultHttpContext { RequestServices = _serviceProvider };
            var actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor());
            var viewResult = _razorViewEngine.FindView(actionContext, viewName, false);

            return viewResult.View;
        }
    }
}