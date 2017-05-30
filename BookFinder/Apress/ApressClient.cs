using BookFinder.Interfaces;
using BookFinder.Model;
using HtmlAgilityPack;

namespace BookFinder.Apress
{
    public class ApressClient : IBookSearch
    {
        private const string BaseUrl = "http://www.apress.com/gp/book/";

        public Book SearchBook(string isbn13)
        {
            var url = BaseUrl + isbn13;
            var pageToSearch = new HtmlWeb();
            var htmlDoc = pageToSearch.Load(url);
            var htmlParser = new ApressHtmlResultParser(htmlDoc);

            return htmlParser.Book; 
        }
         
    }
   
}
