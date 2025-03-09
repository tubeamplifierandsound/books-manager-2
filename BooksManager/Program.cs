using BooksManager.Lib.Models;
using BooksManager.Lib.Services;
using BooksManager.Lib.Storage;

class Program {
    static void Main() {
        List<Book> books;
        string storePath = "..\\..\\..\\Storage\\BookStorage";
        if (!File.Exists(storePath)) {
            Console.WriteLine($"File \"{storePath}\" doesn't exists");
            return;
        }
        BookListService bookService = new BookListService(new BinaryBookListStorage(storePath));
        books = bookService.GetBooks();
        PrintBooks(books);
        FillBookList(bookService);
        books = bookService.GetBooks();
        PrintBooks(books);

    }

    private static void PrintBooks(List<Book> books) {
        Console.WriteLine($"Number of books: {books.Count}");
        foreach (Book book in books) { 
            Console.WriteLine($"ISBN: {book.ISBN}");
            Console.WriteLine($"AuthorName: {book.AuthorName}");
            Console.WriteLine($"BookName: {book.BookName}");
            Console.WriteLine($"Publisher: {book.Publisher}");
            Console.WriteLine($"PublYear: {book.PublYear}");
            Console.WriteLine($"PagesNumber: {book.PagesNumber}");
            Console.WriteLine($"Price: {book.Price}");
            Console.WriteLine();
        }
    }

    private static void FillBookList(BookListService bs) {
        Random rand = new Random();
        for (int i = 1; i < 1000; i++) {
            try {
                bs.AddBook(new Book($"123-4-56-{i:D6}-0", $"Author{rand.Next(1, 50)}", $"Book{i}",
                $"Publisher{rand.Next(1, 11)}", rand.Next(1950, 2025), rand.Next(50, 1000),
                (decimal)Math.Round(rand.NextDouble() * 99 + 1, 2)));
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
        }
    }
}