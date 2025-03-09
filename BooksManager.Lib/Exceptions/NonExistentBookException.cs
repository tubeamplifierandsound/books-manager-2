using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksManager.Lib.Exceptions
{
    public class NonExistentBookException : Exception
    {
        public NonExistentBookException() : base("There is no such book in the collection") { }
        public NonExistentBookException(string message) : base(message) { }
        public NonExistentBookException(string message, Exception exception) : base(message, exception) { }

    }
}
