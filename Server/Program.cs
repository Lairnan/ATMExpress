using System.Net;
using System.Net.Sockets;
using System.Text;

var tcpListener = new TcpListener(IPAddress.Any, 8888);

var isStopped = false;

try
{
	tcpListener.Start();
	Console.WriteLine("Server is started");
	Console.WriteLine("Wait for connection...");

	var readKeyTask = Task.Run(async () =>
	{
		Console.WriteLine("Press <Enter> to stop server");
		while (true)
		{
			if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Enter)
			{
				isStopped = true;
				break;
			}

			await Task.Delay(1000);
		}
	});

	while (true)
	{
		var acceptTask = tcpListener.AcceptTcpClientAsync();

		var completedTask = await Task.WhenAny(acceptTask, readKeyTask);

		if (completedTask == acceptTask)
		{
			var tcpClient = await acceptTask;
			var idClient = Guid.NewGuid();
			Console.WriteLine($"Client connected: {idClient}");

			await Task.Run(async () => await ProcessClientAsync(tcpClient, idClient));

			Console.WriteLine("Press <Enter> to stop server");
			Console.WriteLine("Wait for connection...");
		}
		else if (completedTask == readKeyTask)
		{
			if (isStopped)
				break;
		}
	}
}
finally
{
	tcpListener.Stop();
}

Console.WriteLine("Press <Enter> to continue");
Console.ReadLine();
return;

async Task ProcessClientAsync(TcpClient tcpClient, Guid idClient)
{
    await using var stream = tcpClient.GetStream();
	var buffer = new byte[256];
	var response = new StringBuilder();

	var length = await stream.ReadAsync(buffer);
	response.AppendLine(Encoding.UTF8.GetString(buffer, 0, length));
	Console.WriteLine(response);

	var name = response.ToString().Trim();
	name = name.Split().Last();

	var data = Encoding.UTF8.GetBytes($"Hi {name}! Your id: {idClient}");
	await stream.WriteAsync(data, 0, data.Length);
}