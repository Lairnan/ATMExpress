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
    private readonly IOutput _output;

    public Server(ILogger logger, IOutput output, TcpClient? tcpClient = null)
    {
        _tcpClient = tcpClient ?? new TcpClient();
        _logger = logger;
        _output = output;
    }

    public async Task Connect(IPAddress address, int port)
    {
        try
        {
            await _tcpClient.ConnectAsync(address, port);
            _output.Print("Connection success\n");
        }
        catch (SocketException se)
        {
				await _logger.Log(se.Message, LogType.Error);
        }
    }

    public void Disconnect()
    {
			_tcpClient.Close();
			_output.Print("Disconection success\n");
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
