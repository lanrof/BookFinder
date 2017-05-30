using Newtonsoft.Json;
using System;

namespace BookFinder.Model.Apress
{
    public class ApressBook
    {
        public string Isbn { get; set; }
        [JsonProperty("pPriceGross")]
        public string PriceGrossPapper { get; set; }
        [JsonProperty("ePriceGross")]
        public string PriceGrossEBook { get; set; }
        public string Description { get; set; }
        public string Currency { get; set; }
        public string Url { get; set; }
        public string Photo { get; set; }
        public Ecommerce Ecommerce { get; set; }

        public double BestPrice
        {
            get
            {                
                var eBookPrice = Convert.ToDouble(PriceGrossEBook);
                var paperPrice = Convert.ToDouble(PriceGrossPapper);

                return eBookPrice < paperPrice ? eBookPrice : paperPrice;
            }
        }
    }
}
