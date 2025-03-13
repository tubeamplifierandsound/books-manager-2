using BooksManager.Lib.Services;
using BooksManager.Lib.Models;
using BooksManager.Lib.Storage;

using BooksManager.LoggingServices;
using BooksManager.Lib.Exceptions;

using Moq;

namespace BooksManager.Tests
{
    [TestClass]
    public sealed class BookListServiceTests
    {
        [TestMethod]
        public void AddBook_WhenBookIsUnique_ShouldAddBook()
        {
            // Arrange
            var mockStorage = new Mock<IBookStorage>();
            var mockLogger = new Mock<ILogger>();
            var bookService = new BookListService(mockStorage.Object, mockLogger.Object);
            Book book = new Book("000-0-00-000000-0", "Author_0", 
                "Book_0", "Publisher_0", 0, 0, 0);
            // Act
            bookService.AddBook(book);
            // Assert
            Assert.IsTrue(bookService.Books.Contains(book));
        }

        [TestMethod]
        public void AddBook_WhenBookIsNotUnique_ShouldThrowDuplicateBookException() {
            // Arrange
            var mockStorage = new Mock<IBookStorage>();
            var mockLogger = new Mock<ILogger>();
            var bookService = new BookListService(mockStorage.Object, mockLogger.Object);
            Book book = new Book("000-0-00-000000-0", "Author_0",
                "Book_0", "Publisher_0", 0, 0, 0);
            bookService.AddBook(book);

            // Act and assert
            Assert.ThrowsException<DuplicateBookException>(() => bookService.AddBook(book));
        }

        [TestMethod]
        public void RemoveBook_WhenBookIsContained_ShouldRemoveBook() {
            // Average
            var mockStorage = new Mock<IBookStorage>();
            var mockLogger = new Mock<ILogger>();
            var bookService = new BookListService(mockStorage.Object, mockLogger.Object);
            Book book = new Book("000-0-00-000000-0", "Author_0",
                "Book_0", "Publisher_0", 0, 0, 0);
            bookService.AddBook(book);
            // Act
            bookService.RemoveBook(book);
            // Assert
            Assert.IsFalse(bookService.Books.Contains(book));
        }

        [TestMethod]
        // another way to catch exception in test
        [ExpectedException(typeof(NonExistentBookException))]
        public void RemoveBook_WhenBookIsNotContained_ShouldTrowNonExistentBookException() {
            // Arrange
            var mockStorage = new Mock<IBookStorage>();
            var mockLogger = new Mock<ILogger>();
            var bookService = new BookListService(mockStorage.Object, mockLogger.Object);
            Book book = new Book("000-0-00-000000-0", "Author_0",
                "Book_0", "Publisher_0", 0, 0, 0);
            // Act and assert
            bookService.RemoveBook(book);
        }

        [TestMethod]
        public void SortBooksByTag_ASCByISBN_ShouldSortBooks() {
            // Average
            var mockStorage = new Mock<IBookStorage>();
            var mockLogger = new Mock<ILogger>();
            var bookService = new BookListService(mockStorage.Object, mockLogger.Object);
            Book book1 = new Book("000-0-00-000000-1", "Author_0",
                "Book_1", "Publisher_0", 0, 0, 0);
            Book book2 = new Book("000-0-00-000000-2", "Author_0",
                "Book_2", "Publisher_0", 0, 0, 0);
            Book book3 = new Book("000-0-00-000000-3", "Author_0",
                "Book_3", "Publisher_0", 0, 0, 0);
            bookService.AddBook(book3);
            bookService.AddBook(book1);
            bookService.AddBook(book2);
            // Act
            bookService.SortBooksByTag(BookFields.ISBN, true);
            // Assert
            Assert.AreEqual(book1, bookService.Books[0]);
            Assert.AreEqual(book2, bookService.Books[1]);
            Assert.AreEqual(book3, bookService.Books[2]);
        }

        [TestMethod]
        public void FindBookByTag_ByBookName_ShouldFindAllBooks() {
            const string findName = "Book_1";
            var mockStorage = new Mock<IBookStorage>();
            var mockLogger = new Mock<ILogger>();
            var bookService = new BookListService(mockStorage.Object, mockLogger.Object);
            Book book1 = new Book("000-0-00-000000-1", "Author_0",
               findName, "Publisher_0", 0, 0, 0);
            Book book1_2 = new Book("000-0-00-000000-2", "Author_0",
                findName, "Publisher_0", 0, 0, 0);
            Book book2 = new Book("000-0-00-000000-3", "Author_0",
               "Book_2", "Publisher_0", 0, 0, 0);
            bookService.AddBook(book1);
            bookService.AddBook(book2);
            bookService.AddBook(book1_2);
            // Act
            List<Book> foundBooks = bookService.FindBookByTag(findName, BookFields.BookName);
            // Assert
            Assert.AreEqual(2, foundBooks.Count);
            Assert.AreEqual(findName, foundBooks[0].BookName);
            Assert.AreEqual(findName, foundBooks[1].BookName);
        }


    }
}
