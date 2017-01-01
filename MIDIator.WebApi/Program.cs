using System;
using System.Diagnostics;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.Owin.Hosting;
using MIDIator.Engine;
using MIDIator.Json;
using Newtonsoft.Json;
using Refigure;
using Serilog;

namespace MIDIator.Web
{
	public class Program
	{
		public static WebAPIManager WebAPIManager = new WebAPIManager();
		public static VirtualMIDIManager VirtualMIDIManager = new VirtualMIDIManager();

		public static void Main(string[] args)
		{
			string baseAddress = Config.Get("WebAPI.BaseAddress");
			
            Log.Logger = new LoggerConfiguration()
                .WriteTo.ColoredConsole()
                .CreateLogger();

            using (WebApp.Start<Startup>(baseAddress))
            {
                WebAPIManager.InitializeWebAPI(VirtualMIDIManager, null);
	            Console.WriteLine($"Server running at {baseAddress}. Hit any key to exit.");
				Console.ReadLine();
			}

			WebAPIManager.DisposeWebAPI();
			VirtualMIDIManager.Dispose();
		}
	}
}
