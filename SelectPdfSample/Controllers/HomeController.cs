using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SelectPdf;
using SelectPdfSample.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SelectPdfSample.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Report()
        {
            byte[] report = GeneratePdf();
            return this.File(report, "application/pdf");
        }

        public byte[] GeneratePdf()
        {
            //SelectPdf.GlobalProperties.LicenseKey = "";
            HtmlToPdf htmlToPdf = new HtmlToPdf();
            htmlToPdf.Options.PdfPageOrientation = PdfPageOrientation.Portrait;

            htmlToPdf.Options.PdfPageSize = PdfPageSize.A4;
            htmlToPdf.Options.PageBreaksEnhancedAlgorithm = true;

            htmlToPdf.Options.DisplayHeader = true;
            htmlToPdf.Header.Height = 20;
            htmlToPdf.Header.DisplayOnFirstPage = false;

            htmlToPdf.Options.KeepImagesTogether = true;
            htmlToPdf.Options.DisplayFooter = true;

            GlobalProperties.EnableRestrictedRenderingEngine = true;

            // page numbers can be added using a PdfTextSection object
            PdfTextSection left = new PdfTextSection(10, 10, $"(Versão )", new System.Drawing.Font("Arial", 7));
            left.HorizontalAlign = PdfTextHorizontalAlign.Left;
            htmlToPdf.Footer.Add(left);


            PdfTextSection right = new PdfTextSection(555, 10, "Pág. {page_number}/{total_pages}  ", new System.Drawing.Font("Arial", 7));
            /*right.HorizontalAlign = PdfTextHorizontalAlign.Right*/
            ;
            htmlToPdf.Footer.Add(right);

            string url = "https://jwt.io/";

            PdfDocument pdfDocument = htmlToPdf.ConvertUrl(url);

            byte[] pdf = pdfDocument.Save();

            pdfDocument.Close();
            return pdf;
        }
    }
}
