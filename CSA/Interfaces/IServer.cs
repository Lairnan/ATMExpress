using System.Net;
using System.Text;

namespace CSA.Interfaces;

public interface IServer : IDisposable
{
	public Task Connect(IPAddress iPAddress, int port);
	public void Disconnect();

	public Task SendObject(object obj);
	public Task<object> ReceiveObject(object obj);

	public Task SendMessage(string msg);
	public Task<StringBuilder> ReceivedMessage();


}
