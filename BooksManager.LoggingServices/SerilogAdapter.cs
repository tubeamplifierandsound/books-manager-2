using Serilog;
namespace BooksManager.LoggingServices
{
    public class SerilogAdapter : ILogger
    {
        private readonly Serilog.Core.Logger _logger;
        public SerilogAdapter(string logPath) {
            _logger = new LoggerConfiguration()
                .WriteTo.File(logPath)
                .CreateLogger();
        }
        public void LogInfo(string message) => _logger.Information(message);
        public void LogWarning(string message) => _logger.Warning(message);
        public void LogError(string message, Exception? ex = null) => _logger.Error(ex, message);
    }
}
