// See https://aka.ms/new-console-template for more information
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

Console.WriteLine("Hello, World!");

var tcpClient = new TcpClient();
try
{
	await tcpClient.ConnectAsync("127.0.0.1", 8888);
	using var stream = tcpClient.GetStream();

	var buffer = new byte[256];
	var response = new StringBuilder();

	var data = Encoding.UTF8.GetBytes("My name is Alex");
	await stream.WriteAsync(data, 0, data.Length);

	var recieved = await stream.ReadAsync(buffer);
	response.AppendLine(Encoding.UTF8.GetString(buffer, 0, recieved));
	Console.WriteLine(response);
}
finally
{
	tcpClient.Close();
}

Console.Write("Press any key to continue");
Console.ReadKey();