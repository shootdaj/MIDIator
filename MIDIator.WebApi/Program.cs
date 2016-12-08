using System;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.Owin.Hosting;
using Refigure;

namespace MIDIator.Web
{
	class Program
	{
		static void Main(string[] args)
		{
			string baseAddress = Config.Get("WebApi.BaseAddress");

			using (WebApp.Start<Startup>(baseAddress))
			{
                Console.WriteLine($"Server running at {baseAddress}. Hit any key to exit.");
				Console.ReadLine();
			}
		}

		public static int Iteration { get; set; } = 2;
	}
}
