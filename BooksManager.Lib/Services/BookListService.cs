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
    using SelectFunc = Func<Book, IComparable>;
    internal class BookListService
    {
        private List<Book> _books;
        private IBookStorage _storage;
        private TaskQueue _threadPool;

        public BookListService(IBookStorage storage) { 
            _storage = storage ?? throw new ArgumentNullException(nameof(storage));
            LoadList();

            int threadNum = Environment.ProcessorCount;
            _threadPool = new TaskQueue(threadNum);
        }

        public void AddBook(Book book) {
            if (book == null) { 
                throw new ArgumentNullException(nameof(book));
            }
            //if (_books.Contains(book)) {
            //    throw new DuplicateBookException();
            //}
            if (FindBookByTag(book).Count != 0) {
                throw new DuplicateBookException();
            }
            _books.Add(book);
        }

        public void RemoveBook(Book book) {
            //if (!_books.Remove(book)) {
            //    throw new NonExistentBookException();
            //}
            if (FindBookByTag(book).Count == 0) {
                throw new NonExistentBookException();
            }
            _books.Remove(book);
        }

        public List<Book> FindBookByTag(object searchVal, BookFields? searchTag = null) {
            List<Book> found = new List<Book>();

            SelectFunc selectFunc = GetSelector(searchTag);

            int booksPerThread = 100; //*
            int taskNum = _books.Count / booksPerThread;
            int booksExcess = _books.Count % booksPerThread;

            int startInd = 0;

            for (int i = 0; i < taskNum; i++) {
                MakePartlySearch(searchVal, selectFunc, found, startInd, startInd + booksPerThread);
                startInd += booksPerThread;
            }
            if (booksExcess > 0) { 
                MakePartlySearch(searchVal, selectFunc, found, startInd, startInd + booksExcess);
            }
            return found;
        }

        private void MakePartlySearch(object searchVal, SelectFunc selectValMethod, List<Book> found, int startInd, int searchBorder) {
            TaskQueue.TaskDelegate task;
            task = () => {
                for (int i = startInd; i < searchBorder; i++)
                {
                    Book checkBook = _books[i];
                    if (searchVal.Equals(selectValMethod(checkBook)))
                    {
                        lock (found)
                        {
                            found.Add(checkBook);
                        }
                    }
                }
            };
            _threadPool.EnqueueTask(task);
        }

        public void SortBooksByTag(BookFields sortTag, bool ascending)
        {
            var selector = GetSelector(sortTag);
            if (ascending)
            {
                _books = _books.OrderBy(selector).ToList();
            }
            else {
                _books = _books.OrderByDescending(selector).ToList();
            }
        }

        private SelectFunc GetSelector(BookFields? tag) {
            if (tag == null) {
                return (Book book) => (IComparable)book;
            }
            return tag switch
            {
                BookFields.ISBN => (book => book.ISBN),
                BookFields.AuthorName => (book => book.AuthorName),
                BookFields.BookName => (book => book.BookName),
                BookFields.Publisher => (book => book.Publisher),
                BookFields.PublYear => (book => book.PublYear),
                BookFields.PagesNumber => (book => book.PagesNumber),
                BookFields.Price => (book => book.Price),
                _ => throw new ArgumentException("Incorrect sort tag - ", nameof(tag))
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
