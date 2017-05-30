using BookFinder.Model;
using BookFinder.Utils;
using System;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;

namespace BookFinder.Amazon
{
    public class AmazonXmlResultParser
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
        private XDocument _xmlResult;
        private static XNamespace AmazonWebServiceNamespace = "http://webservices.amazon.com/AWSECommerceService/2011-08-01";
        #endregion

        public AmazonXmlResultParser(string xmlResult)
        {
            _xmlResult = XDocument.Parse(xmlResult);        
        }

        #region private methods
        private Book GetBook()
        {
            Book book = null;

            try
            {
                var price = GetBestPrice();
                var url = GetOfferUrl();
                var title = GetTitle();

                book = BookFactory.CreateBook(price, url, title);
            }
            catch(Exception ex)
            {
                Debug.WriteLine("Error during book conversion from Amazon service. \nError:\n" + ex.Message);
            }

            return book;
        } 

        private Price GetBestPrice()
        {
            var price = new Price();
            var priceNode = _xmlResult.Descendants(GetTag("LowestNewPrice")).FirstOrDefault();
            price.Cost = Convert.ToDouble(priceNode.Descendants(GetTag("Amount")).FirstOrDefault().Value) / 100;

            var currency = priceNode.Descendants(GetTag("CurrencyCode")).FirstOrDefault().Value;
            price.Currency = (Currency)Enum.Parse(typeof(Currency), currency, true);

            return price;
        }

        private string GetOfferUrl()
        {
            return _xmlResult.Descendants(GetTag("DetailPageURL")).FirstOrDefault().Value;
        }

        private string GetTitle()
        {
            return _xmlResult.Descendants(GetTag("Title")).FirstOrDefault().Value;
        }

        private XName GetTag(string nodeName)
        {
            return AmazonWebServiceNamespace + nodeName;
        }
        #endregion
    }
}
