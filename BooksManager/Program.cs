using BooksManager.ConsoleApp;
using BooksManager.Lib.Models;
using BooksManager.Lib.Services;
using BooksManager.Lib.Storage;
using System;
using System.Diagnostics;
using static System.Reflection.Metadata.BlobBuilder;

class Program {
    static void Main() {
        const string storePath = "..\\..\\..\\Storage\\BookStorage";
        BookListService bookService;
        if (!File.Exists(storePath)) {
            Console.WriteLine($"File \"{storePath}\" doesn't exists");
            return;
        }
        bookService = new BookListService(new BinaryBookListStorage(storePath));

        Demonstrator.DemonstrateWork(bookService);

    }

}