using BookDataGenerator.Models;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.Text;

namespace BookDataGenerator.Services
{
    public class DataExportService
    {
        public byte[] ExportToCsv(List<Book> books)
        {
            using var memoryStream = new MemoryStream();
            using var writer = new StreamWriter(memoryStream, Encoding.UTF8);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

            // Create flattened objects for CSV export
            var csvRecords = books.Select(book => new BookCsvRecord
            {
                Index = book.Index,
                ISBN = book.ISBN,
                Title = book.Title,
                Author = book.Author,
                Publisher = book.Publisher,
                Genre = book.Genre,
                Price = book.Price,
                Pages = book.Pages,
                PublicationDate = book.PublicationDate,
                Likes = book.Likes,
                ReviewCount = book.Reviews,
                Description = book.Description,
                CoverImageUrl = book.CoverImageUrl,
                AverageRating = book.ReviewDetails.Count > 0 
                    ? Math.Round(book.ReviewDetails.Average(r => r.Rating), 1) 
                    : 0,
                ReviewTexts = string.Join(" | ", book.ReviewDetails.Select(r => $"{r.Author} ({r.Rating}★): {r.Text}")),
                FirstReviewDate = book.ReviewDetails.Count > 0 
                    ? book.ReviewDetails.Min(r => r.ReviewDate) 
                    : (DateTime?)null,
                LastReviewDate = book.ReviewDetails.Count > 0 
                    ? book.ReviewDetails.Max(r => r.ReviewDate) 
                    : (DateTime?)null
            }).ToList();

            csv.WriteRecords(csvRecords);
            writer.Flush();
            return memoryStream.ToArray();
        }
    }

    public class BookCsvRecord
    {
        public int Index { get; set; }
        public string ISBN { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string Publisher { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Pages { get; set; }
        public DateTime PublicationDate { get; set; }
        public int Likes { get; set; }
        public int ReviewCount { get; set; }
        public string Description { get; set; } = string.Empty;
        public string CoverImageUrl { get; set; } = string.Empty;
        public double AverageRating { get; set; }
        public string ReviewTexts { get; set; } = string.Empty;
        public DateTime? FirstReviewDate { get; set; }
        public DateTime? LastReviewDate { get; set; }
    }
}