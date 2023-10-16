namespace CSA.Interfaces
{
	public interface ILogger
	{
		Task Log(string message, LogType logType);
	}

	public enum LogType
	{
		Error,
		Warning,
		Information,
	}
}
