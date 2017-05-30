using BookFinder.Model;
using BookFinder.Model.Apress;
using BookFinder.Utils;
using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Linq;

namespace BookFinder.Apress
{
    public class ApressHtmlResultParser
    {
        #region properties
        public Book Book
        {
            get
            {
                if(_book == null)
                {
                    _book = GetBook();
                }
                return _book;
            }
        }
        #endregion

        #region fields
        private Book _book;
        private HtmlDocument _htmlDocument;
        #endregion

        public ApressHtmlResultParser(HtmlDocument htmlDocument)
        {
            _htmlDocument = htmlDocument;
        }

        private Book GetBook()
        {
            ApressBook apressBook = null;
            try
            {
                var json = _htmlDocument
                    .DocumentNode
                    .Descendants("script")
                    .Where(y => y.InnerHtml.Contains("\n   dataLayer ="))
                    .FirstOrDefault().InnerText;

                json = json.Remove(0, 17);
                json = json.Remove(json.Length - 3, 2);
                apressBook = JsonConvert.DeserializeObject<ApressBook>(json);                
            }
            catch(Exception ex)
            {
                Debug.WriteLine("Error during book conversion from Apress service. \nError:\n" + ex.Message);
            }

            return BookFactory.CreateBook(apressBook);
        }

    }
}
