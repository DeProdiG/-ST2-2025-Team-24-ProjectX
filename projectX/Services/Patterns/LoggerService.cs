// LoggerService.cs
namespace projectX.Services.Patterns
{
    public interface ILoggerService
    {
        void LogInformation(string message);
        void LogWarning(string message);
        void LogError(string message);
        List<string> GetLogs();
    }

    public class LoggerService : ILoggerService
    {
        private static LoggerService _instance;
        private static readonly object _lock = new object();
        private readonly List<string> _logs;

        private LoggerService()
        {
            _logs = new List<string>();
        }

        public static LoggerService Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new LoggerService();
                        }
                    }
                }
                return _instance;
            }
        }

        public void LogInformation(string message)
        {
            var logEntry = $"[INFO] {DateTime.Now:yyyy-MM-dd HH:mm:ss}: {message}";
            _logs.Add(logEntry);
            Console.WriteLine(logEntry);
        }

        public void LogWarning(string message)
        {
            var logEntry = $"[WARN] {DateTime.Now:yyyy-MM-dd HH:mm:ss}: {message}";
            _logs.Add(logEntry);
            Console.WriteLine(logEntry);
        }

        public void LogError(string message)
        {
            var logEntry = $"[ERROR] {DateTime.Now:yyyy-MM-dd HH:mm:ss}: {message}";
            _logs.Add(logEntry);
            Console.WriteLine(logEntry);
        }

        public List<string> GetLogs()
        {
            return new List<string>(_logs);
        }
    }
}