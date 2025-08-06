using BookDataGenerator.Models;
using BookDataGenerator.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookDataGenerator.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExportController : ControllerBase
    {
        private readonly BookGeneratorService _bookGenerator;
        private readonly DataExportService _exportService;

        public ExportController(BookGeneratorService bookGenerator, DataExportService exportService)
        {
            _bookGenerator = bookGenerator;
            _exportService = exportService;
        }

        [HttpGet("csv")]
        public IActionResult ExportCsv(
            [FromQuery] string language = "en-US",
            [FromQuery] string seed = "default",
            [FromQuery] double avgLikes = 5.0,
            [FromQuery] double avgReviews = 5.0,
            [FromQuery] int pages = 1,
            [FromQuery] int pageSize = 20)
        {
            try
            {
                var allBooks = new List<Book>();
                
                // Generate books for the specified number of pages
                for (int page = 1; page <= pages; page++)
                {
                    var books = _bookGenerator.GenerateBooks(
                        language,
                        page,
                        pageSize,
                        avgLikes,
                        avgReviews,
                        seed);
                    allBooks.AddRange(books);
                }

                var csvBytes = _exportService.ExportToCsv(allBooks);
                var fileName = $"books-{language}-{DateTime.Now:yyyyMMdd-HHmmss}-{pages}pages-{allBooks.Count}records.csv";
                
                return File(csvBytes, "text/csv", fileName);
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<string>.Fail($"Export failed: {ex.Message}"));
            }
        }

        [HttpGet("csv/current")]
        public IActionResult ExportCurrentView(
            [FromQuery] string language = "en-US",
            [FromQuery] string seed = "default",
            [FromQuery] double avgLikes = 5.0,
            [FromQuery] double avgReviews = 5.0,
            [FromQuery] int currentPage = 1)
        {
            try
            {
                var allBooks = new List<Book>();
                
                // Export all pages that user has currently loaded (from page 1 to currentPage)
                for (int page = 1; page <= currentPage; page++)
                {
                    var pageSize = page == 1 ? 20 : 10; // First page has 20, others have 10
                    var books = _bookGenerator.GenerateBooks(
                        language,
                        page,
                        pageSize,
                        avgLikes,
                        avgReviews,
                        seed);
                    allBooks.AddRange(books);
                }

                var csvBytes = _exportService.ExportToCsv(allBooks);
                var fileName = $"books-current-view-{language}-{DateTime.Now:yyyyMMdd-HHmmss}-{allBooks.Count}records.csv";
                
                return File(csvBytes, "text/csv", fileName);
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<string>.Fail($"Export failed: {ex.Message}"));
            }
        }
    }
}