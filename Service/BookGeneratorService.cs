using BookDataGenerator.Models;

namespace BookDataGenerator.Services
{
    public class BookGeneratorService
    {
        private readonly DataLoaderService _dataLoader;
        
        private readonly string[] _genres = { "Fiction", "Mystery", "Romance", "Sci-Fi", "Fantasy", "Thriller", "Biography", "History", "Self-Help", "Technical" };
        
        private readonly string[] _reviewTexts = {
            "This book changed my perspective completely!",
            "Couldn't put it down - read it all night!",
            "The author's best work by far.",
            "Perfect for a lazy weekend read.",
            "The ending was unexpected but satisfying.",
            "Characters felt incredibly real and relatable.",
            "A masterpiece of modern literature.",
            "Great storytelling and character development.",
            "Would definitely recommend to friends.",
            "An absolute page-turner from start to finish."
        };

        private readonly string[] _bookDescriptions = {
            "A compelling narrative that explores the depths of human nature and the complexities of modern life.",
            "An unforgettable journey through landscapes both physical and emotional, revealing truths about love and loss.",
            "A gripping tale that weaves together mystery and adventure in unexpected ways.",
            "A profound exploration of family, identity, and the bonds that tie us together.",
            "An epic story spanning generations, filled with unforgettable characters and stunning revelations.",
            "A thought-provoking work that challenges conventional wisdom and opens new perspectives.",
            "A beautifully written story of redemption, hope, and the power of human connection.",
            "An intricate plot filled with twists and turns that will keep readers guessing until the very end."
        };

        public BookGeneratorService(DataLoaderService dataLoader)
        {
            _dataLoader = dataLoader;
        }

        public List<Book> GenerateBooks(string language, int page, int pageSize, double avgLikes, double avgReviews, string seed)
        {
            var books = new List<Book>();
            var rngSeed = seed.GetHashCode() + page;
            var rng = new Random(rngSeed);

            var titles = _dataLoader.GetTitles(language);
            var authors = _dataLoader.GetAuthors(language);
            var publishers = _dataLoader.GetPublishers(language);

            for (int i = 0; i < pageSize; i++)
            {
                int index = (page - 1) * pageSize + i + 1;
                var title = titles[rng.Next(titles.Count)];
                var author = authors[rng.Next(authors.Count)];
                var publisher = publishers[rng.Next(publishers.Count)];
                
                // Generate proper ISBN (simplified)
                var isbn = $"978{rng.Next(1000000, 9999999)}{rng.Next(100, 999)}";
                
                int likes = Math.Max(0, (int)Math.Round(avgLikes + (rng.NextDouble() - 0.5) * 4));
                int reviewsCount = Math.Max(0, (int)Math.Round(avgReviews + (rng.NextDouble() - 0.5) * 3));

                // Generate reviews
                var reviewDetails = new List<Review>();
                for (int r = 0; r < reviewsCount; r++)
                {
                    var reviewAuthor = authors[rng.Next(authors.Count)];
                    var reviewText = _reviewTexts[rng.Next(_reviewTexts.Length)];
                    var rating = rng.Next(3, 6); // 3-5 stars
                    var reviewDate = DateTime.Now.AddDays(-rng.Next(1, 365));

                    reviewDetails.Add(new Review 
                    { 
                        Author = reviewAuthor, 
                        Text = reviewText,
                        Rating = rating,
                        ReviewDate = reviewDate,
                        Company = $"Reader{rng.Next(1000, 9999)}"
                    });
                }

                // Generate book details
                var genre = _genres[rng.Next(_genres.Length)];
                var price = Math.Round(rng.NextDouble() * 25 + 5, 2); // $5-30
                var pages = rng.Next(150, 800);
                var pubDate = DateTime.Now.AddDays(-rng.Next(30, 3650));
                var description = _bookDescriptions[rng.Next(_bookDescriptions.Length)];
                var coverImageUrl = $"https://picsum.photos/200/300?random={index}";

                books.Add(new Book
                {
                    Index = index,
                    ISBN = isbn,
                    Title = title,
                    Author = author,
                    Publisher = publisher,
                    Likes = likes,
                    Reviews = reviewsCount,
                    ReviewDetails = reviewDetails,
                    CoverImageUrl = coverImageUrl,
                    Price = (decimal)price,
                    Pages = pages,
                    PublicationDate = pubDate,
                    Genre = genre,
                    Description = description
                });
            }
            return books;
        }
    }
}