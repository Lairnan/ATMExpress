using System.Net;
using System.Text;

namespace CSA.Interfaces;

public interface IServer : IDisposable
{
	public Task<bool> Connect(IPAddress iPAddress, int port);
	public bool Disconnect();

	public Task<bool> SendObject(object obj);
	public Task<object?> ReceiveObject();

	public Task<bool> SendMessage(string msg);
	public Task<StringBuilder?> ReceivedMessage();


}
