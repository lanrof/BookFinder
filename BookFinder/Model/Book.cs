
namespace BookFinder.Model
{
    public class Book
    {
        public Price Price { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
        public string Market { get; set; }
        public string ISBN13 { get; set; }
        public string Author { get; set; }

        public Book(string url, string title, string market, string isbn13, string author, Price price)
        {
            Price = price;
            Url = url;
            Title = title;
            Market = market;
            ISBN13 = isbn13;
            Author = author; 
        }
    }
}
