// See https://aka.ms/new-console-template for more information
using CSA.Implements;
using CSA.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Net;

var logger = new Logger();

var serviceProvider = new ServiceCollection()
	.AddLogging()
	.AddSingleton<ILogger, Logger>()
	.AddTransient<IServer, Server>()
	.BuildServiceProvider();

logger.WriteLog("Starting application...", LogType.Information, ConsoleColor.White);

using var server = serviceProvider.GetService<IServer>();
await server.Connect(IPAddress.Parse("127.0.0.1"), 8888);
await server.SendMessage("Test");

//Console.WriteLine("Hello, World!");

/*var tcpClient = new TcpClient();
try
{
	var server = new Server(tcpClient);
	server.Connect(IPAddress.Parse("127.0.0.1"), 8888);
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
}*/

Console.Write("Press any key to continue");
Console.ReadKey();