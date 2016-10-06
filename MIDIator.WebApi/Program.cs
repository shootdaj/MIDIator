using System;
using Microsoft.Owin.Hosting;

namespace MIDIator.Web
{
	class Program
	{
		static void Main(string[] args)
		{
			string baseAddress = "http://localhost:9000/";

			using (WebApp.Start<Startup>(baseAddress))
			{
				Console.WriteLine(MIDIManager.One);
				Console.WriteLine($"Server running at {baseAddress}. Hit any key to exit.");
				Console.ReadLine();
			}
		}
	}
}
