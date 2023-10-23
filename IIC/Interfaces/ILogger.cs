namespace IIC.Interfaces;

public interface ILogger
{
	void Info(string message);
	void Warning(string message);
	void Success(string message);
	void Error(Exception message);
}

public enum LogType
{
	Error,
	Warning,
	Information,
	Success,
}