using System.Text;
using IIC.Interfaces;
using Microsoft.Extensions.Configuration;

namespace IIC.Implements;

public class Logger : ILogger
{
	private readonly string _logFilePath;
	private static readonly Dictionary<LogType, ConsoleColor> LogTypeColors = new()
    {
		{ LogType.Error, ConsoleColor.DarkRed },
		{ LogType.Warning, ConsoleColor.Yellow },
		{ LogType.Information, ConsoleColor.White },
		{ LogType.Success, ConsoleColor.Green },
	};

	public Logger()
	{
		var dateString = DateTime.Now.ToString("ddMMyyyy");

		var configuration = new ConfigurationBuilder()
			.SetBasePath(Directory.GetCurrentDirectory() + "/config")
			.AddJsonFile("logger.json", true, true)
			.Build();

		var path = new StringBuilder(configuration["AppSettings:logPath"]);
		if(!Directory.Exists(path.ToString())) Directory.CreateDirectory(path.ToString());

		path.Append($"{dateString}.txt");

		_logFilePath = path.ToString();
    }

    public void Info(string message)
    {
        CheckLogFilePath();
        PrintLog(message, LogType.Information);
    }

    public void Warning(string message)
    {
        CheckLogFilePath();
        PrintLog(message, LogType.Warning);
    }

    public void Success(string message)
    {
        CheckLogFilePath();
        PrintLog(message, LogType.Success);
    }

    public void Error(Exception exception)
    {
        CheckLogFilePath();
        PrintLog(exception.ToString(), LogType.Error);
	}

    private void PrintLog(string message, LogType logType)
    {
        try
        {
            CheckFileExistAndPrint(_logFilePath, logType, message);
        }
        catch (Exception ex)
        {
            CheckFileExistAndPrint(_logFilePath, LogType.Error, ex.ToString());
        }
    }

    private void CheckLogFilePath()
    {
        if (!string.IsNullOrWhiteSpace(_logFilePath)) return;
        
        var consoleColor = Console.ForegroundColor;
        Console.ForegroundColor = LogTypeColors[LogType.Error];
        Console.WriteLine($"[{LogType.Error}] {DateTime.Now, 0:dd/MM/yyyy HH:mm} Path could not be empty");
        Console.ForegroundColor = consoleColor;
        throw new ArgumentNullException(nameof(_logFilePath));
    }

	private static async void CheckFileExistAndPrint(string logFilePath, LogType logType, string message)
	{
		var consoleColor = Console.ForegroundColor;
		if (!File.Exists(logFilePath))
			File.Create(logFilePath).Close();

		try
		{
			Console.ForegroundColor = LogTypeColors[logType];

            await using var writer = new StreamWriter(logFilePath, true);
			Console.WriteLine($"[{logType}] {DateTime.Now, 0:dd/MM/yyyy HH:mm} {message}");
			await writer.WriteLineAsync($"[{logType}] {DateTime.Now, 0:dd/MM/yyyy HH:mm} {message}");
			await writer.DisposeAsync();
		}
		catch (Exception ex)
		{
			Console.ForegroundColor = LogTypeColors[LogType.Error];
			Console.WriteLine($"[{LogType.Error}] {DateTime.Now, 0:dd/MM/yyyy HH:mm} {ex}");
		}
		Console.ForegroundColor = consoleColor;
	}
}
