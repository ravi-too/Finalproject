using System.Text.Json;

public class Logger
{
    private const string LogFile = "logs.json";

    public class LogEntry
    {
        public string Message { get; set; }
        public string Date { get; set; }
    }

    public static void Log(string message)
    {
        List<LogEntry> logs;

        if (File.Exists(LogFile))
        {
            string json = File.ReadAllText(LogFile);
            logs = JsonSerializer.Deserialize<List<LogEntry>>(json) ?? new List<LogEntry>();
        }
        else
        {
            logs = new List<LogEntry>();
        }

        logs.Add(new LogEntry
        {
            Message = message,
            Date = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss")
        });

        string output = JsonSerializer.Serialize(logs, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(LogFile, output);
    }
}
