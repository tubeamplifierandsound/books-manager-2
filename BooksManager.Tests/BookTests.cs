using BooksManager.Lib.Models;

namespace BooksManager.Tests
{
    [TestClass]
    public sealed class BookTests
    {
        [TestMethod]
        public void ToString_WhenCompleteFormat_ShouldReturnCorrectFormattedString() {
            // Average
            Book book = new Book("000-0-00-000000-0", "Author_0",
               "Book_0", "Publisher_0", 0, 0, 0);
            string expectedString = "ISBN 13: 000-0-00-000000-0, Author_0, Book_0, \"Publisher_0\", 0, P. 0, 0,00 Br";
            // Act
            string formattedBook = book.ToString("C");
            // Assert
            Assert.AreEqual(expectedString, formattedBook);
        }

        [TestMethod]
        public void Equals_WhenCompareWithEqual_ShouldReturnTrue()
        {
            // Average 
            Book book = new Book("000-0-00-000000-0", "Author_0",
               "Book_0", "Publisher_0", 0, 0, 0);
            bool areEqual;
            // Act
            areEqual = book.Equals(book);
            // Assert
            Assert.IsTrue(areEqual);
        }

        [TestMethod]
        public void Equals_WhenCompareWithNotEqual_ShouldReturnFalse()
        {
            // Average 
            Book book1 = new Book("000-0-00-000000-0", "Author_0",
               "Book_0", "Publisher_0", 0, 0, 0);
            Book book2 = new Book("000-0-00-000000-1", "Author_0",
               "Book_1", "Publisher_0", 0, 0, 0);
            bool areEqual;
            // Act
            areEqual = book1.Equals(book2);
            // Assert
            Assert.IsFalse(areEqual);
        }
    }
}
