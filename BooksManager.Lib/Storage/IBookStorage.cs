using BooksManager.Lib.Models;

namespace BooksManager.Lib.Storage
{
    public interface IBookStorage
    {
        public void SaveBooks(IEnumerable<Book> books);
        public List<Book> LoadBooks();
    }
}
