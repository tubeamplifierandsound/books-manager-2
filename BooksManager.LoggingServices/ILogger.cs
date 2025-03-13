using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksManager.LoggingServices
{
    public interface ILogger
    {
        public void LogInfo(string infoMess);
        public void LogWarning(string warningMess);
        public void LogError(string errorMess, Exception? ex = null);
    }
}
