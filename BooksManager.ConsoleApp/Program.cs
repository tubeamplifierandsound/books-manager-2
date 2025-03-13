using BooksManager.ConsoleApp;
using BooksManager.Lib.Models;
using BooksManager.Lib.Services;
using BooksManager.Lib.Storage;
using BooksManager.LoggingServices;
using System;
using System.Diagnostics;
using static System.Reflection.Metadata.BlobBuilder;

class Program {
    static void Main() {
        const string storePath = "..\\..\\..\\AppData\\Storage\\BookStorage";
        const string logPath = "..\\..\\..\\AppData\\Logs\\log.txt";
        ILogger _logger = new SerilogAdapter(logPath);
        BookListService bookService;
        if (!File.Exists(storePath)) {
            Console.WriteLine($"File \"{storePath}\" doesn't exists");
            return;
        }
        bookService = new BookListService(new BinaryBookListStorage(storePath), _logger);

        Demonstrator.DemonstrateWork(bookService);

    }
}