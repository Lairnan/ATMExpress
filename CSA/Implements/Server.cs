using CSA.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CSA.Implements;

public class Server : IServer
{
    private readonly TcpClient _tcpClient;
    private readonly ILogger _logger;

    public Server(ILogger logger, TcpClient? tcpClient = null)
    {
        _tcpClient = tcpClient ?? new TcpClient();
        _logger = logger;
    }

    public async Task Connect(IPAddress address, int port)
    {
        try
        {
            await _tcpClient.ConnectAsync(address, port);
            _logger.WriteLog("Connection success", LogType.Success, ConsoleColor.Green);
        }
        catch (SocketException se)
        {
			_logger.Error(se);
        }
    }

    public void Disconnect()
    {
		_tcpClient.Close();
		_logger.WriteLog("Disconection success", LogType.Success, ConsoleColor.Green);
	}

    public void Dispose() => Disconnect();

    public async Task<StringBuilder> ReceivedMessage()
    {
        return default;
    }

    public async Task<object> ReceiveObject(object obj)
    {
        return default;
    }

    public async Task SendMessage(string msg)
    {
    }

    public async Task SendObject(object obj)
    {
    }
}
