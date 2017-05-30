using BookFinder.Model;

namespace BookFinder.Interfaces
{
    public interface IBookSearch
    {
        Book SearchBook(string isbn13);          
    }
}
