using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using System.IO;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace PDFCore.Services
{
    public class RazorRendererHelper : IRazorRendererHelper
    {
        private IRazorViewEngine _viewEngine;
        private ITempDataProvider _tempDataProvider;
        private IServiceProvider _serviceProvider;

        public RazorRendererHelper(IRazorViewEngine viewEngine, ITempDataProvider tempDataProvider, IServiceProvider serviceProvider)
        {
            _viewEngine = viewEngine;
            _tempDataProvider = tempDataProvider;
            _serviceProvider = serviceProvider;
        }

        public string RenderPartialToString<TModel>(string razorViewName, TModel model)
        {
            var actionContext = GetActionContext();
            // Get razor view
            var razor = FindView(actionContext, razorViewName);

            using (var output = new StringWriter())
            {
                var viewContext = new ViewContext(
                    actionContext,
                    razor,
                    new ViewDataDictionary<TModel>(
                        metadataProvider: new EmptyModelMetadataProvider(),
                        modelState: new ModelStateDictionary())
                    {
                        Model = model
                    },
                    new TempDataDictionary(
                        actionContext.HttpContext,
                        _tempDataProvider),
                    output,
                    new HtmlHelperOptions()
                   );

                razor.RenderAsync(viewContext).ConfigureAwait(false);

                return output.ToString();
            }
        }

        public IView FindView(ActionContext actionContext, string razorViewName)
        {
            var getRazorViewResult = _viewEngine.GetView(null, razorViewName, false);
            if (getRazorViewResult.Success)
            {
                return getRazorViewResult.View;
            }

            var findRazorViewResult = _viewEngine.FindView(actionContext, razorViewName, false);
            if (findRazorViewResult.Success)
            {
                return findRazorViewResult.View;
            }

            var searchedLocations = getRazorViewResult.SearchedLocations.Concat(findRazorViewResult.SearchedLocations);
            var error = string.Join(
                Environment.NewLine,
                new[] { $"Unable to find razor view '{razorViewName}'. The following locations were searched:" }.Concat(searchedLocations));
            throw new InvalidOperationException(error);
        }

        private ActionContext GetActionContext()
        {
            var httpContext = new DefaultHttpContext
            {
                RequestServices = _serviceProvider,
            };
            return new ActionContext(httpContext, new RouteData(), new ActionDescriptor());
        }
    }
}
