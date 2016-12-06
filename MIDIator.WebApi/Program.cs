using System;
using Microsoft.AspNet.SignalR.Client;
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
                var hubConnection = new HubConnection(baseAddress);
                IHubProxy eventHubProxy = hubConnection.CreateHubProxy("EventHub");
                eventHubProxy.On<string, ChannelEvent>("OnEvent", (channel, ev) => Log.Information("Event received on {channel} channel - {@ev}", channel, ev));
                hubConnection.Start().Wait();

                Console.WriteLine($"Server running at {baseAddress}. Hit any key to exit.");
				Console.ReadLine();
			}
		}

		public static int Iteration { get; set; } = 2;
	}
}
