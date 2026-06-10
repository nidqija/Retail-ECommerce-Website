using System;
using System.IO;

namespace RetailECommerce.Services.Logging;

public class AdminLogger
{
    private static readonly object _lock = new object();
    private static AdminLogger? _instance;
    private readonly string _logFilePath;

    private AdminLogger()
    {
        _logFilePath = Path.Combine(Directory.GetCurrentDirectory(), "admin_logs.txt");
        try
        {
            File.WriteAllText(_logFilePath, $"--- Log Session Started: {DateTime.Now:yyyy-MM-dd HH:mm:ss} ---{Environment.NewLine}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Logger Error] Failed to initialize log file: {ex.Message}");
        }
    }

    public static AdminLogger Instance
    {
        get
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = new AdminLogger();
                }
                return _instance;
            }
        }
    }

    public void Log(string message)
    {
        string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}";

        // Log to terminal (Console)
        Console.WriteLine(logEntry);

        // Log to txt file
        try
        {
            lock (_lock)
            {
                File.AppendAllText(_logFilePath, logEntry + Environment.NewLine);
                
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Logger Error] Failed to write to log file: {ex.Message}");
        }
    }
}
