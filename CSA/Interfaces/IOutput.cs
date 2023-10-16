namespace CSA.Interfaces
{
	public interface IOutput
	{
		void Print(string message);
		void Print(string message, Exception exception);
		void Print(Exception exception);
	}
}
