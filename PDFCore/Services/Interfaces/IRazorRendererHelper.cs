using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace PDFCore.Services
{
    public interface IRazorRendererHelper
    {
        IView FindView(ActionContext actionContext, string razorViewName);
        string RenderPartialToString<TModel>(string razorViewName, TModel model);
    }
}