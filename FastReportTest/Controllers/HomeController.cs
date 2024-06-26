using FastReport;
using Microsoft.AspNetCore.Mvc;
using Testing.Models;
using FastReport.Web;
using FastReport.Export.PdfSimple;
namespace FastReportTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly IHostEnvironment _env;

        public HomeController(IHostEnvironment env)
        {
            _env = env ?? throw new ArgumentNullException(nameof(env));
        }

        [HttpGet("hello")]
        public IActionResult ViewReport()
        {
            List<HomeModel> sales = new List<HomeModel>();
            sales.Add(new HomeModel() { Id = "1", Name = "TEST" });
            sales.Add(new HomeModel() { Id = "2", Name = "TEST" });
            Report webReport = new Report();
            try
            {
                string path = Path.Combine(_env.ContentRootPath, "Reports/test.frx");
                if (!System.IO.File.Exists(path))
                {
                    return StatusCode(404, "Report file not found.");
                }
                webReport.Report.Load(path);

                // Register the data source
                webReport.Report.RegisterData(sales, "EmployeeRef");
                if (webReport.Report.Prepare())
                {
                    var pdfExport = new PDFSimpleExport();
                    System.IO.MemoryStream ms = new System.IO.MemoryStream();
                    webReport.Report.Export(pdfExport, ms);
                    ms.Position = 0;
                    if (ms.Length == 0)
                    {
                        return StatusCode(500, "Exported PDF is empty.");
                    }
                    byte[] byteArray = ms.ToArray();
                    string base64Data = Convert.ToBase64String(byteArray);
                    return Ok(new { Base64Pdf = base64Data });
                }
                else
                {
                    return Ok(new { Ber = "ไม่ได้หว่ะ" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}

