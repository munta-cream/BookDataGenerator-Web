namespace BookDataGenerator.Models
{
    public class Review
    {
        public string Text { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
        public int Rating { get; set; } = 5;
        public DateTime ReviewDate { get; set; } = DateTime.Now;
    }

    public class Book
    {
        public int Index { get; set; }
        public string ISBN { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string Publisher { get; set; } = string.Empty;
        public int Likes { get; set; }
        public int Reviews { get; set; }
        public List<Review> ReviewDetails { get; set; } = new List<Review>();
        public string CoverImageUrl { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public DateTime PublicationDate { get; set; }
        public string Genre { get; set; } = string.Empty;
        public int Pages { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}