using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using PDFCore.Templates;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFCore.Services
{
    public class PdfService : IPdfService
    {
        private readonly IConverter _converter;
        private readonly IRazorRendererHelper _rendererHelper;

        public PdfService(IConverter convereter, IRazorRendererHelper rendererHelper)
        {
            _converter = convereter;
            _rendererHelper = rendererHelper;
        }

        public byte[] GeneratePdfFromRazorView()
        {
            var employeePdf = new EmployeePdfTemplate
            {
                Name = "Emyr",
                LastName = "Griffiths",
                Age = 38,
                Gender = "Male"
            };

            var razorTemplate = "/Views/PdfCreator/CreatePdf.cshtml";
            // Convert razor file to string 
            var htmlContent = _rendererHelper.RenderPartialToString(razorTemplate, employeePdf);

            return GeneratePdf(htmlContent);
        }

        private byte[] GeneratePdf(string htmlContent)
        {
            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 10 },
                DocumentTitle = "PDF Report",
            };

            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = htmlContent,
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "assets", "styles.css") },
                HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = true },
                FooterSettings = { FontName = "Arial", FontSize = 9, Line = true, Center = "Report Footer" }
            };

            var pdf = new HtmlToPdfDocument
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings },
            };

            var file = _converter.Convert(pdf);

            return file;
        }

    }
}
