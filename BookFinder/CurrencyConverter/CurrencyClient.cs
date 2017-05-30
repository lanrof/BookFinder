using BookFinder.Model;
using BookFinder.Model.CurrencyRates;
using RestSharp;

namespace BookFinder.CurrencyConverter
{
    public class CurrencyClient
    {
        private const string BaseUrl = "http://api.fixer.io/";
        private RestClient _client;

        public CurrencyClient()
        {
            _client = new RestClient(BaseUrl);
        }

        public CurrencyCourse GetCurrencyCourses(Currency forCurrency)
        {
            var request = new RestRequest("latest", Method.GET);
            request.AddParameter("base", forCurrency.ToString());
            var response = _client.Execute<CurrencyCourse>(request);

            return response.Data;
        }
    }
}
