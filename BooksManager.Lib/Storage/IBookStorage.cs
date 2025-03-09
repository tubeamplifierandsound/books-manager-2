using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BooksManager.Lib.Models;

namespace BooksManager.Lib.Storage
{
    internal interface IBookStorage
    {
        public void SaveBooks(IEnumerable<Book> books);
        public List<Book> LoadBooks();
    }
}
