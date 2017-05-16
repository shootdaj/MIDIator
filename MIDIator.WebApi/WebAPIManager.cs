using System;
using System.Collections.Generic;
using System.IO;
using Anshul.Utilities;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Client;
using MIDIator.Engine;
using MIDIator.Json;
using MIDIator.Services;
using Newtonsoft.Json;
using Refigure;
using Serilog;

namespace MIDIator.Web
{
	public class WebAPIManager
	{
		private HubConnection HubConnection { get; set; }

		private VirtualMIDIManager VirtualMIDIManager { get; set; }

		public SignalRService SignalRService => SignalRService.Instance;

		private IDisposable ShutdownSub { get; set; }

        public void InitializeWebAPI(VirtualMIDIManager virtualMIDIManager, Action onShutdown)
		{
            //serialization
			var settings = SerializerSettings.DefaultSettings;
			settings.ContractResolver = new SignalRContractResolver(settings.ContractResolver);
			settings.TypeNameHandling = TypeNameHandling.Auto;
			var serializer = JsonSerializer.Create(settings);
			GlobalHost.DependencyResolver.Register(typeof(JsonSerializer), () => serializer);

            VirtualMIDIManager = virtualMIDIManager;
            
			//start signalr
			StartSignalR();
            //initialize managers
			InitMIDIManager();
            AdminManager.Instantiate();

            ShutdownSub = AdminManager.Instance.OnShutdown.Subscribe(s => onShutdown());
        }

		private void InitMIDIManager()
		{
		    var midiDeviceService = new MIDIDeviceService();
			SignalRService.Instantiate();
			MIDIManager.Instantiate(midiDeviceService,
				new ProfileService(midiDeviceService, VirtualMIDIManager, SignalRService.SetBroadcastAction),
				VirtualMIDIManager);

		    if (string.IsNullOrEmpty(Config.Get("WebAPI.ProfileFile")) || !File.Exists(Config.Get("WebAPI.ProfileFile")))
		    {
                //set initial profile if none is found
                SetInitialProfile();
            }
		    else
		    {
			    var text = File.ReadAllText(Config.Get("WebAPI.ProfileFile"));

				if (text.Trim() == string.Empty)// TODO: || text.IsValidFormat?
					SetInitialProfile();
				else
				{
					var profile = JsonConvert.DeserializeObject<Profile>(text);
					MIDIManager.Instance.SetProfile(profile);
				}
            }
		}

	    private void SetInitialProfile()
		{
			var profile = new Profile()
			{
				Name = "DefaultProfile",
				Transformations = new List<Transformation>(),
				VirtualLoopbackDevices = new BetterList<VirtualLoopbackDevice>()
			};

			MIDIManager.Instance.SetProfile(profile);
		}

		private void StartSignalR()
		{
			// start hubs
			HubConnection = new HubConnection(Config.Get("WebAPI.BaseAddress"));
			IHubProxy eventHubProxy = HubConnection.CreateHubProxy("MIDIReaderHub");
			eventHubProxy.On<string, ChannelEvent>("OnEvent",
				(channel, ev) => Log.Information("Event received on {channel} channel - {@ev}", channel, ev));
			HubConnection.Start().Wait();

			eventHubProxy.Invoke("Subscribe", Constants.AdminChannel);
			eventHubProxy.Invoke("Subscribe", Constants.TaskChannel);

			// start service
			SignalRService.Instantiate();
		}

		public void DisposeWebAPI()
		{
            ShutdownSub.Dispose();
            AdminManager.Instance.Dispose();
			MIDIManager.Instance.Dispose();
			HubConnection.Stop();
			HubConnection.Dispose();
			HubConnection = null;
		}
	}
}
