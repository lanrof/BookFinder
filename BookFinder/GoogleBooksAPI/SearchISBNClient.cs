using BookFinder.Model;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;

namespace BookFinder.GoogleBooksAPI
{
    public class SearchISBNClient
    {
        private const string BaseUrl = "http://www.isbnsearch.org/";

        public List<Book> SearchISBN(string bookTitle)
        {
            var url = BaseUrl + "search?s=" + bookTitle;
            var pageToSearch = new HtmlWeb();
            var htmlDoc = pageToSearch.Load(url);
            var books = Parse(htmlDoc);

            return books;
        }

        private List<Book> Parse(HtmlDocument htmlDoc)
        {
            throw new NotImplementedException();
        }
    }
}
