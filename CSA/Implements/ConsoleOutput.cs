using CSA.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSA.Implements;

public class ConsoleOutput : IOutput
{
	public void Print(string message)
	{
		Console.Write(message);
	}
}
