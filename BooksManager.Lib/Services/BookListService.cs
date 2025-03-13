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
    public class BookListService
    {
        private List<Book> _books;
        private IBookStorage _storage;
        private TaskQueue _threadPool;

        public IReadOnlyList<Book> Books => _books; 

        public BookListService(IBookStorage storage) { 
            _storage = storage ?? throw new ArgumentNullException(nameof(storage));
            LoadList();
            if (_books == null) { 
                _books = new List<Book>();
            }
            int threadNum = Environment.ProcessorCount;
            _threadPool = new TaskQueue(threadNum);
        }

        public void AddBook(Book book) {
            if (book == null) { 
                throw new ArgumentNullException(nameof(book));
            }
            //if (_books.Contains(book))
            //{
            //    throw new DuplicateBookException();
            //}
            if (FindBookByTag(book).Count != 0)
            {
                throw new DuplicateBookException();
            }
            _books.Add(book);
        }

        public void RemoveBook(Book book) {
            //if (!_books.Remove(book)) {
            //    throw new NonExistentBookException();
            //}
            //mb get index in FindBookByTag and delete by index
            if (FindBookByTag(book).Count == 0) {
                throw new NonExistentBookException();
            }
            _books.Remove(book);
        }

        public List<Book> FindBookByTag(object searchVal, BookFields? searchTag = null) {
            int booksPerThread = 20_000;

            //const int partsNum = 1;
            //int booksPerThread = _books.Count / partsNum; //20000; //*

            //if (booksPerThread == 0)
            //{
            //    booksPerThread = partsNum;
            //}

            List<Book> found = new List<Book>();

            SelectFunc? selectFunc = GetSelector(searchTag);

            int taskNum = _books.Count / booksPerThread;
            int booksExcess = _books.Count % booksPerThread;

            int totalTaskNum = booksExcess > 0 ? taskNum + 1 : taskNum;
            CountdownEvent countdown = new CountdownEvent(totalTaskNum);

            int startInd = 0;

            for (int i = 0; i < taskNum; i++) {
                MakePartlySearch(searchVal, selectFunc, found, startInd, startInd + booksPerThread, countdown);
                startInd += booksPerThread;
            }
            if (booksExcess > 0) { 
                MakePartlySearch(searchVal, selectFunc, found, startInd, startInd + booksExcess, countdown);
            }
            countdown.Wait();
            return found;
        }

        private void MakePartlySearch(object searchVal, SelectFunc? selectValMethod, List<Book> found, 
            int startInd, int searchBorder, CountdownEvent countdown) {
            TaskQueue.TaskDelegate task;
            task = () => {
                List<Book> localFound = new List<Book>();
                for (int i = startInd; i < searchBorder; i++)
                {
                    Book checkBook = _books[i];
                    bool areMatch = selectValMethod != null ?
                    searchVal.Equals(selectValMethod(checkBook)) :
                    searchVal.Equals(checkBook);

                    if (areMatch)
                    {
                        localFound.Add(checkBook);
                    }
                }
                lock (found)
                {
                    found.AddRange(localFound);
                }
                countdown.Signal();
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

        private SelectFunc? GetSelector(BookFields? tag) {
            if (tag == null) {
                return null;
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


        public List<Book> GetBooksCopy() {
            return new List<Book>(_books);
        }

    }
}
