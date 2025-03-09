using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksManager.Lib.Exceptions
{
    public class DuplicateBookException : Exception
    {
        public DuplicateBookException() : base("Book is already exists in collection") { }
        public DuplicateBookException(string message) : base(message) { }
        public DuplicateBookException(string message, Exception exception) : base(message, exception) { }
    
    }
}
