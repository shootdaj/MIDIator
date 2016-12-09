using System;
using System.Diagnostics;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.Owin.Hosting;
using MIDIator.Json;
using Newtonsoft.Json;
using Refigure;
using Serilog;

namespace MIDIator.Web
{
	class Program
	{
		static void Main(string[] args)
		{
			string baseAddress = Config.Get("WebApi.BaseAddress");

            Log.Logger = new LoggerConfiguration()
                .WriteTo.ColoredConsole()
                .CreateLogger();

            using (WebApp.Start<Startup>(baseAddress))
            {
                var settings = SerializerSettings.DefaultSettings;
                settings.ContractResolver = new SignalRContractResolver(settings.ContractResolver);
                settings.TypeNameHandling = TypeNameHandling.Auto;
                var serializer = JsonSerializer.Create(settings);
                GlobalHost.DependencyResolver.Register(typeof(JsonSerializer), () => serializer);

                //start signalr hubs
                var hubConnection = new HubConnection(Config.Get("WebApi.BaseAddress"));
                IHubProxy eventHubProxy = hubConnection.CreateHubProxy("MIDIReaderHub");
                eventHubProxy.On<string, ChannelEvent>("OnEvent", (channel, ev) => Log.Information("Event received on {channel} channel - {@ev}", channel, ev));
                hubConnection.Start().Wait();

                eventHubProxy.Invoke("Subscribe", Constants.AdminChannel);
                eventHubProxy.Invoke("Subscribe", Constants.TaskChannel);
                Console.WriteLine($"Server running at {baseAddress}. Hit any key to exit.");
				Console.ReadLine();
			}
		}

		public static int Iteration { get; set; } = 2;
	}
}
