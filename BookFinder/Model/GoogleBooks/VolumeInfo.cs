using System.Collections.Generic;

namespace BookFinder.Model.GoogleBooks
{
    public class VolumeInfo
    {
        public string Title { get; set; }
        public List<string> Authors { get; set; }
        public string PublishedDate { get; set; }
        public string Description { get; set; }
        public List<IndustryIdentifier> IndustryIdentifiers { get; set; }
        public ImageLinks ImageLinks { get; set; }
        public string PreviewLink { get; set; }

        public string ISBN13
        {
            get {
                if (IndustryIdentifiers == null)
                    return "";

                var isbn13 = IndustryIdentifiers.Find(x => x.Type == "ISBN_13");
                return isbn13 != null ? isbn13.Identifier : "";
            }            
        }
    }
}
