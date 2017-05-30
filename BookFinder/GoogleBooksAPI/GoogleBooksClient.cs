using BookFinder.Model.GoogleBooks;
using RestSharp;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookFinder.GoogleBooksAPI
{
    public class GoogleBooksClient
    {
        private const string BaseUrl = "https://www.googleapis.com/books/v1";
        private RestClient _client;

        public GoogleBooksClient()
        {
            _client = new RestClient(BaseUrl);
        }

        public Task<List<VolumeInfo>> SearchBookByTitle(string title, int maxResult = 40, int startIndex = 0)
        {
            return Task.Run(() =>
           {
               var request = PrepareRequest(title, maxResult, startIndex);
               var response = _client.Execute<ResponseRootObject>(request);
               var volumesInfo = new List<VolumeInfo>();

               foreach (var item in response.Data.Items)
               {
                   volumesInfo.Add(item.VolumeInfo);
               }

               return volumesInfo;
           });
        }

        private RestRequest PrepareRequest(string title, int maxResult, int startIndex)
        {
            var request = new RestRequest("volumes", Method.GET);
            var formattedTitle = "intitle:" + title;            
            request.AddParameter("q", formattedTitle);
            request.AddParameter("maxResults", maxResult);
            request.AddParameter("startIndex", startIndex);

            return request;
        }
    }
}
