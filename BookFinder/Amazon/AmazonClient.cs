using BookFinder.Interfaces;
using System.Collections.Generic;
using BookFinder.Model;
using Nager.AmazonProductAdvertising;
using Nager.AmazonProductAdvertising.Operation;

namespace BookFinder.Amazon
{
    public class AmazonClient : IBookSearch
    {
        public Book SearchBook(string isbn13)
        {
            var wrapper = PrepareAmazonWrapper();
            var requestParams = PrepareOperation(isbn13);
            var response = wrapper.Request(requestParams);
            var xmlParser = new AmazonXmlResultParser(response.Content);

            return xmlParser.Book;
        }

        private AmazonOperationBase PrepareOperation(string isbn13)
        {                              
            var parameters = new Dictionary<string, string>();
            parameters["Operation"] = "ItemLookup";
            parameters["ResponseGroup"] = "Large";
            parameters["SearchIndex"] = "Books";
            parameters["IdType"] = "ISBN";
            parameters["ItemId"] = isbn13;
            parameters["AssociateTag"] = Configuration.AmazonAssociateTag;

            AmazonOperationBase operation = new AmazonOperationBase();
            operation.ParameterDictionary = parameters;

            return operation;
        }

        private AmazonWrapper PrepareAmazonWrapper()
        {
            var auth = new AmazonAuthentication();
            auth.AccessKey = Configuration.AmazonAPIAccessKey;
            auth.SecretKey = Configuration.AmazonAPISecretKey;

            return new AmazonWrapper(auth, AmazonEndpoint.US, Configuration.AmazonAssociateTag);
        }

    }
}
