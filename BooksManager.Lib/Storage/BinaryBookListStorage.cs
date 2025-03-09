using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BooksManager.Lib.Models;

namespace BooksManager.Lib.Storage
{
    public class BinaryBookListStorage : IBookStorage
    {

        private string _storagePath;
        public BinaryBookListStorage(string? storagePath)
        {
            _storagePath = storagePath ?? throw new ArgumentNullException(nameof(storagePath));
        }

        public void SaveBooks(IEnumerable<Book> books) {
            using (var writer = new BinaryWriter(File.Open(_storagePath, FileMode.Create))) {
                foreach (var book in books) {
                    writer.Write(book.ISBN);
                    writer.Write(book.AuthorName);
                    writer.Write(book.BookName);
                    writer.Write(book.Publisher);
                    writer.Write(book.PublYear);
                    writer.Write(book.PagesNumber);
                    writer.Write(book.Price);
                }
            }
        }

        public List<Book> LoadBooks() {
            var books = new List<Book>();
            if (File.Exists(_storagePath)) {
                using (var reader = new BinaryReader(File.Open(_storagePath, FileMode.Open)))
                {
                    while (reader.BaseStream.Position < reader.BaseStream.Length)
                    {
                        var isbn = reader.ReadString();
                        var authorName = reader.ReadString();
                        var bookName = reader.ReadString();
                        var publisher = reader.ReadString();
                        var publYear = reader.ReadInt32();
                        var pagesNumber = reader.ReadInt32();
                        var price = reader.ReadDecimal();

                        books.Add(new Book(isbn, authorName, bookName, publisher, publYear, pagesNumber, price));
                    }
                }
            }
            
            return books;
        }
    }
}
