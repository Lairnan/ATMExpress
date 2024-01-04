using System.Net;
using System.Net.Sockets;
using System.Text;
using BinaryFormatter;
using IIC.Interfaces;

namespace IIC.Implements;

public class Server : IServer
{
    private readonly TcpClient _tcpClient;
    private readonly ILogger _logger;

    public Server(ILogger logger, TcpClient? tcpClient = null)
    {
        _tcpClient = tcpClient ?? new TcpClient();
        _logger = logger;
    }

    public async Task<bool> Connect(IPAddress address, int port)
    {
        try
        {
            _logger.Info("Tried connect to server");
            await _tcpClient.ConnectAsync(address, port);
            _logger.Success("Connection success");
            return true;
        }
        catch (SocketException se)
        {
			_logger.Error(se);
            return false;
        }
    }

    public bool Disconnect()
    {
        try
        {
            _logger.Info("Tried close connection");
            _tcpClient.Close();
            _logger.Success("Disconnection success");
            return true;
        } catch (SocketException se)
        {
            _logger.Error(se);
            return false;
        }
	}

    public void Dispose() => Disconnect();

    public async Task<StringBuilder?> ReceivedMessage()
    {
        try
        {
            var receive = await Receive();
            if (receive == null) return null;
            var receivedObject = receive.Value;

			var stringBuilder = new StringBuilder();
			stringBuilder.Append(Encoding.UTF8.GetString(receivedObject.Received, 0, receivedObject.LengthReceived));
            return stringBuilder;
		}
		catch (SocketException se)
		{
			_logger.Error(se);
            return null;
		}
		catch (InvalidOperationException ioe)
		{
			_logger.Error(ioe);
			return null;
		}
    }

	public async Task<object?> ReceiveObject()
    {
        try
        {
            var buffer = await Receive();

            if (buffer != null)
            {
                var receivedObject = buffer.Value;
                var jsonStr = Encoding.UTF8.GetString(receivedObject.Received, 0, receivedObject.LengthReceived);
                return jsonStr;
            }

            _logger.Success("Received object success");
            return buffer;
        }
        catch (SocketException se)
        {
            _logger.Error(se);
            return null;
        }
    }

    public async Task<bool> SendMessage(string msg)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(msg)) return false;
            var buffer = Encoding.UTF8.GetBytes(msg);

            return await Send(buffer);
        }
        catch (SocketException se)
        {
            _logger.Error(se);
            return false;
        }
    }

    public async Task<bool> SendObject(object obj)
	{
		try
		{
            var bf = new BinaryConverter();
            var buffer = bf.Serialize(obj);

            return await Send(buffer);
		}
		catch (SocketException se)
		{
			_logger.Error(se);
			return false;
		}
	}

    private async Task<bool> Send(byte[] data)
    {
        try
        {
            await using var stream = _tcpClient.GetStream();
			await stream.WriteAsync(data);
            return true;
		}
        catch (SocketException se)
        {
            _logger.Error(se);
            return false;
        }
        catch (InvalidOperationException ioe)
        {
            _logger.Error(ioe);
            return false;
        }
    }

    private async Task<ReceivedObject?> Receive()
    {
        try
        {
            await using var stream = _tcpClient.GetStream();
            var buffer = new byte[4096];
            var received = await stream.ReadAsync(buffer);
            var receivedObject = new ReceivedObject(buffer, received);
            return receivedObject;
		}
        catch (SocketException se)
        {
            _logger.Error(se);
            return null;
		}
		catch (InvalidOperationException ioe)
		{
			_logger.Error(ioe);
			return null;
		}
	}
}

internal struct ReceivedObject
{
    public byte[] Received { get; set; }
    public int LengthReceived {  get; set; }

    public ReceivedObject(byte[] received, int length)
    {

        this.Received = received ?? throw new ArgumentNullException(nameof(received));
        this.LengthReceived = length;
    }
}