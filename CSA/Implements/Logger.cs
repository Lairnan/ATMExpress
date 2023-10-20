using CSA.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using System.Text;

namespace CSA.Implements;

public class Logger : ILogger
{
	private readonly string logFilePath;

	public Logger()
	{
		var dateString = DateTime.Now.ToString("ddMMyyyy");

		var configuration = new ConfigurationBuilder()
			.SetBasePath(Directory.GetCurrentDirectory() + "/config")
			.AddJsonFile("logger.json", true, true)
			.Build();

		StringBuilder path = new StringBuilder(configuration["AppSettings:logPath"]);
		if(!Directory.Exists(path.ToString())) Directory.CreateDirectory(path.ToString());

		path.Append($"{dateString}.txt");

		logFilePath = path.ToString();
    }

	public void WriteLog(string message, LogType logType = LogType.Information, ConsoleColor userColor = ConsoleColor.White)
	{
		var consoleColor = Console.ForegroundColor;

		if (string.IsNullOrWhiteSpace(logFilePath))
		{
			Console.ForegroundColor = ConsoleColor.DarkRed;
			Console.WriteLine($"[{LogType.Error}]: File Not Nound");
			Console.ForegroundColor = consoleColor;
			return;
		}

		Console.ForegroundColor = userColor;
		try
		{
			CheckFileExistAndPrint(logFilePath, logType, message);
		}
		catch (Exception ex)
		{
			Console.WriteLine($"[{LogType.Error}]: {ex}");
			CheckFileExistAndPrint(logFilePath, LogType.Error, ex.ToString());
		}

		Console.WriteLine($"[{logType}] : {DateTime.Now,0:dd.MM.yyyy HH:mm} {message}");
		Console.ForegroundColor = consoleColor;
	}

	public void Error(Exception exception)
	{
		if (string.IsNullOrWhiteSpace(logFilePath))
		{
			Console.WriteLine($"[{LogType.Error}]: File Not Nound");
			return;
		}

		var consoleColor = Console.ForegroundColor;
		Console.ForegroundColor = ConsoleColor.DarkRed;

		try
		{
			CheckFileExistAndPrint(logFilePath, LogType.Error, exception.ToString());
		}
		catch (Exception ex)
		{
			Console.WriteLine($"[{LogType.Error}]: {ex}");
			CheckFileExistAndPrint(logFilePath, LogType.Error, ex.ToString());
		}

		Console.WriteLine($"[{LogType.Error}] : {DateTime.Now,0:dd.MM.yyyy HH:mm} {exception}");
		Console.ForegroundColor = consoleColor;
	}

	private async void CheckFileExistAndPrint(string logFilePath, LogType logType, string message)
	{
		if (!File.Exists(logFilePath))
			File.Create(logFilePath);

		await Task.Delay(25);

		using var writer = new StreamWriter(logFilePath, true);
		await writer.WriteLineAsync($"[{logType}] : {DateTime.Now,0:dd/MM/yyyy hh:mm tt} : {message}");
		await writer.FlushAsync();
		await writer.DisposeAsync();
	}
}
