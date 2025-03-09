using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BooksManager.Lib.Models;
using BooksManager.Lib.Storage;
using BooksManager.Lib.Exceptions;
using BooksManager.Lib.Concurrent;

namespace BooksManager.Lib.Services
{
    internal class BookListService
    {
        private List<Book> _books;
        private IBookStorage _storage;
        private TaskQueue _threadPool;

        public BookListService(IBookStorage storage) { 
            _storage = storage ?? throw new ArgumentNullException(nameof(storage));
            LoadList();
        }

        public void AddBook(Book book) {
            if (book == null) { 
                throw new ArgumentNullException(nameof(book));
            }
            if (_books.Contains(book)) {
                throw new DuplicateBookException();
            }
            _books.Add(book);
        }

        public void RemoveBook(Book book) {
            if (!_books.Remove(book)) {
                throw new NonExistentBookException();
            }
        }

        public List<Book> FindBookByTag(BookFields findTag) {
            List<Book> found = new List<Book>();

           // int partSize = _books.Count / 

            return found;
        }



        public void SortBooksByTag(BookFields sortTag, bool ascending)
        {
            var selector = GetSortSelector(sortTag);
            if (ascending)
            {
                _books = _books.OrderBy(selector).ToList();
            }
            else {
                _books = _books.OrderByDescending(selector).ToList();
            }
        }

        private Func<Book, IComparable> GetSortSelector(BookFields sortTag) {
            return sortTag switch
            {
                BookFields.ISBN => (book => book.ISBN),
                BookFields.AuthorName => (book => book.AuthorName),
                BookFields.BookName => (book => book.BookName),
                BookFields.Publisher => (book => book.Publisher),
                BookFields.PublYear => (book => book.PublYear),
                BookFields.PagesNumber => (book => book.PagesNumber),
                BookFields.Price => (book => book.Price),
                _ => throw new ArgumentException("Incorrect sort tag - ", nameof(sortTag))
            };
        }



        public void SaveList() {
            _storage.SaveBooks(_books);
        }

        public void LoadList() {
            _books = _storage.LoadBooks();
        }

    }
}
