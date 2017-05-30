using BookFinder.Model;
using BookFinder.Model.Apress;
using System;
using System.Linq;

namespace BookFinder.Utils
{
    public static class BookFactory
    {
        public static Book CreateBook(ApressBook apressBook)
        {
            if (apressBook == null)
                return null;

            var title = apressBook.Ecommerce.Impressions.FirstOrDefault().Name;
            var url = apressBook.Url;
            var price = new Price();
            price.Currency = (Currency)Enum.Parse(typeof(Currency), apressBook.Currency, true);
            price.Cost = apressBook.BestPrice;

            var book = new Book(url, title, "Apress", "", "", price);
            return book;
        }

        public static Book CreateBook(Price price, string url, string title)
        {
            if(price == null || String.IsNullOrEmpty(url) || String.IsNullOrEmpty(title))
            {
                return null;
            }

            var book = new Book(url, title, "Amazon", "", "", price);
            return book;
        }
    }
}
