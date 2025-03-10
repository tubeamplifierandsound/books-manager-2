using BooksManager.Lib.Models;
using BooksManager.Lib.Services;
using BooksManager.Lib.Storage;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BooksManager.ConsoleApp
{
    internal static class Demonstrator
    {
        public const int outputInterval = 100_000;
        
        public static void DemonstrateWork(BookListService bookService, bool initializeFile = false) {
            
            PrintBooksAtIntervals(bookService.Books, "Books after service creation (loaded from file)", outputInterval);


            if (initializeFile) {
                // initialization of storage
                //
                FillBookList(bookService);
                bookService.SaveList();
                PrintBooksAtIntervals(bookService.Books, "Books after initialization", outputInterval);
            }

            // adding unique element 
            //
            Stopwatch sw = Stopwatch.StartNew();
            Book newBook = UniqueBook(bookService.Books[^1]);
            sw.Start();
            try
            {
                bookService.AddBook(newBook);
                sw.Stop();
                Console.WriteLine($"Book (ISBN: {newBook.ISBN}) was added successfully");
                Console.WriteLine($"Search time: {sw.ElapsedMilliseconds} ms");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (sw.IsRunning)
                {
                    sw.Stop();
                }
            }


            // adding not unique element
            //
            Book oldBook = bookService.Books[^1];
            try
            {
                bookService.AddBook(oldBook);
                Console.WriteLine($"Book (ISBN: {oldBook.ISBN}) was added successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


            // deleting existent element
            //
            Book existBook = bookService.Books[0];
            try
            {
                bookService.RemoveBook(existBook);
                Console.WriteLine($"Book (ISBN: {existBook.ISBN}) was deleted successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            // deleting non-existent element
            //
            Book nExistBook = UniqueBook(bookService.Books[^1]);
            try
            {
                bookService.RemoveBook(nExistBook);
                Console.WriteLine($"Book (ISBN: {nExistBook.ISBN}) was deleted successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            PrintBooksAtIntervals(bookService.Books, "Books after adding and deleting", outputInterval);

            // sorting
            //
            bookService.SortBooksByTag(BookFields.ISBN, false);
            PrintBooksAtIntervals(bookService.Books, "Books after sorting by ISBN (desc)", outputInterval);
            //
            bookService.SortBooksByTag(BookFields.Price, false);
            PrintBooksAtIntervals(bookService.Books, "Books after sorting by price (desc)", outputInterval);
            //
            bookService.SortBooksByTag(BookFields.Price, true);
            PrintBooksAtIntervals(bookService.Books, "Books after sorting by price (asc)", outputInterval);


            // search book by tag
            //
            string searchStr = "Publisher_1";
            List<Book> foundBooks = bookService.FindBookByTag(searchStr, BookFields.Publisher);
            PrintBooksAtIntervals(foundBooks, $"Found books with certain publisher ({searchStr})", outputInterval);

            // sting representation of Book
            //
            Console.WriteLine("All ToString formats of 0 element:");
            Console.WriteLine(bookService.Books[0].ToString("G"));
            Console.WriteLine(bookService.Books[0].ToString("M"));
            Console.WriteLine(bookService.Books[0].ToString("F"));
            Console.WriteLine(bookService.Books[0].ToString("C"));

        }



        // methods with specific logic - only for checking correctness of task implementation
        private static string consoleDelimiter = "===================================================================";

        private static void PrintBooksAtIntervals(IReadOnlyList<Book> books, string mess = "", int interval = 1)
        {
            Console.WriteLine(consoleDelimiter);
            Console.WriteLine(mess);
            Console.WriteLine($"Number of books: {books.Count}");
            for (int i = 0; i < books.Count; i += interval)
            {
                Book currBook = books[i];
                Console.WriteLine($"ISBN: {currBook.ISBN}");
                Console.WriteLine($"AuthorName: {currBook.AuthorName}");
                Console.WriteLine($"BookName: {currBook.BookName}");
                Console.WriteLine($"Publisher: {currBook.Publisher}");
                Console.WriteLine($"PublYear: {currBook.PublYear}");
                Console.WriteLine($"PagesNumber: {currBook.PagesNumber}");
                Console.WriteLine($"Price: {currBook.Price}");
                Console.WriteLine();
            }
            Console.WriteLine(consoleDelimiter);
        }

        private static Book UniqueBook(Book last)
        {
            // take number
            int uniqueNum = GetNumFromISBN(last.ISBN) + 1;
            return CreateBookByNum(uniqueNum);

        }

        private static void FillBookList(BookListService bs)
        {
            const int partsNum = 7;
            Stopwatch sw = Stopwatch.StartNew();
            sw.Start();
            try
            {
                int count = 0;
                for (int j = 0; j < partsNum; j++)
                {
                    for (int i = 1; i < 100_000; i++)
                    {
                        bs.AddBook(CreateBookByNum(count++));
                    }
                    Console.WriteLine($"{(j + 1)}/{partsNum}, elapsed time from the start: {sw.Elapsed.ToString(@"hh\:mm\:ss\:fff")}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                sw.Stop();
            }
        }

        private static string GetISBNFormNum(int num)
        {
            return string.Format("{0:000-0-00-000000-0}", num);
        }
        private static int GetNumFromISBN(string str)
        {
            str = str.Replace("-", "");
            return (int)long.Parse(str);
        }
        private static Book CreateBookByNum(int num)
        {
            Random rand = new Random();
            string isbn = GetISBNFormNum(num);
            return new Book(isbn, $"Author_{rand.Next(1, 1000)}", $"Book_{num}",
                    $"Publisher_{rand.Next(1, 10)}", rand.Next(1950, 2025), rand.Next(50, 1000),
                    (decimal)Math.Round(rand.NextDouble() * 99 + 1, 2));
        }
    }
}
