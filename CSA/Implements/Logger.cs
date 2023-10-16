using CSA.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSA.Implements
{
	public class Logger : ILogger
	{
		private string logFilePath;
		private IOutput _output;

		public Logger(IOutput output, string? path = null)
		{
			this._output = output;
			var dateString = DateTime.Now.ToString("ddMMyyyy");

			if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
				logFilePath = $"log/{dateString}.txt";
			else logFilePath = path;
        }

		public async Task Log(string message, LogType logType)
		{
			if (string.IsNullOrWhiteSpace(logFilePath) || !File.Exists(logFilePath))
			{
				_output.Print($"[{LogType.Error}]: File Not Nound\n");
				return;
			}

			try
			{
				if(!File.Exists(logFilePath))
					File.Create(logFilePath);

				using var writer = new StreamWriter(logFilePath, true);
				await writer.WriteLineAsync($"[{logType}]: {DateTime.Now,0:dd.MM.yyyy HH:mm} {message}");
				await writer.FlushAsync();
				await writer.DisposeAsync();
			}
			catch (Exception ex)
			{
				_output.Print($"[{LogType.Error}]: {ex.Message}\n");
			}

			_output.Print($"[{logType}]: {DateTime.Now,0:dd.MM.yyyy HH:mm} {message}\n");
		}
	}
}
