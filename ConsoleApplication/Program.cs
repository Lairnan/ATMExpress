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

logger.Info("Starting application...");

{
    using var server = serviceProvider.GetService<IServer>()!;
    await server.Connect(IPAddress.Parse("127.0.0.1"), 8888);
    await server.SendMessage("Test");
}

Console.Write("Press any key to continue");
Console.ReadKey();

Console.Write("\nClosing application...");
await Task.Delay(1000);