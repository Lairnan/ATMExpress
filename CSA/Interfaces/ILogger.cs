namespace CSA.Interfaces;

public interface ILogger
{
	void WriteLog(string message, LogType logType, ConsoleColor userColor);
	void Error(Exception message);
}

public enum LogType
{
	Error,
	Warning,
	Information,
	Success,
}
