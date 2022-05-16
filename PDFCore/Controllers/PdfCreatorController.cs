using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PDFCore.Services;
using PDFCore.Utility;
using System.IO;

namespace PDFCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PdfCreatorController : ControllerBase
    {
        private readonly IConverter _converter;
        private readonly IPdfService _pdfService;

        public PdfCreatorController(IConverter converter, IPdfService pdfService)
        {
            _converter = converter;
            _pdfService = pdfService;
        }

        [HttpGet]
        public IActionResult CreatePdf()
        {
            var pdf = _pdfService.GeneratePdfFromRazorView();
            return File(pdf, "application/octet-stream", "RazorPdf.pdf");
        }
    }
}
