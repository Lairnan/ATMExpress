using CSA.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CSA.Implements
{
    public class Server : IServer
    {
        private readonly TcpClient _tcpClient;

        public Server(TcpClient? tcpClient = null)
        {
            _tcpClient = tcpClient ?? new TcpClient();
        }

        public async Task Connect(IPAddress address, int port)
        {
            try
            {
                await _tcpClient.ConnectAsync(address, port);
            }
            catch (SocketException se)
            {
                Trace.WriteLine(se.Source);
            }
        }

        public void Disconnect()
        {
            _tcpClient.Close();
        }

        public void Dispose()
        {
            Disconnect();
            _tcpClient?.Dispose();
        }

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
}
