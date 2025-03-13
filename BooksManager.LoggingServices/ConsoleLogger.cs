namespace BooksManager.LoggingServices
{
    public class ConsoleLogger : ILogger
    {
        public void LogInfo(string message) {
            Console.WriteLine("Log (Info): " + message);
        }
        public void LogWarning(string message) {
            Console.WriteLine("Log (Warning): " + message);
        }
        public void LogError(string message, Exception? ex = null)
        {
            Console.WriteLine($"Error: {message}; {ex?.Message}");
        }
    }
}
