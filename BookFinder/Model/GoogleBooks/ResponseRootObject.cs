using System.Collections.Generic;

namespace BookFinder.Model.GoogleBooks
{
    class ResponseRootObject
    {
        public int TotalItems { get; set; }
        public List<Item> Items { get; set; }
    }
}
