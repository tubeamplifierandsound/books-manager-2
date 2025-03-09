﻿namespace BooksManager.Lib.Models
{
    public class Book : IEquatable<Book>, IComparable<Book>
    {
        public string ISBN { get; set; }
        public string AuthorName { get; set; }
        public string BookName { get; set; }
        public string Publisher { get; set; }
        public int PublYear { get; set; }
        public int PagesNumber { get; set; }
        public decimal Price { get; set; }

        public Book(string isbn, string authName, string bookName,
            string publisher, int publYear, int pagesNumber, decimal price)
        {
            ISBN = isbn;
            AuthorName = authName;
            BookName = bookName;
            Publisher = publisher;
            PublYear = publYear;
            PagesNumber = pagesNumber;
            Price = price;
        }

        public bool Equals(Book? other) {
            if (other == null) {
                return false;
            }
            if (this.ISBN == other.ISBN)
            {
                return true;
            }
            else { 
                return false;
            }
        }

        public override bool Equals(Object? obj) {
            if (obj == null) {
                return false;
            }
            Book book = obj as Book;
            if (book == null)
            {
                return false;
            }
            else {
                return this.Equals(book);
            }
        }

        public override int GetHashCode() {
            return this.ISBN.GetHashCode();
        }

        public static bool operator == (Book book1, Book book2) {
            if (((object)book1) == null || ((object)book2) == null)
            {
                return Object.Equals(book1, book2);
            }
            else { 
                return book1.Equals(book2);
            }
        }

        public static bool operator !=(Book book1, Book book2) {
            if (((object)book1) == null || ((object)book2) == null)
            {
                return !Object.Equals(book1, book2);
            }
            else {
                return !(book1.Equals(book2));
            }
        }

        public int CompareTo(Book? book) {
            if (book == null) {
                return 1;
            }
            else {
                return this.ISBN.CompareTo(book.ISBN);
            }
        }
    }
}
