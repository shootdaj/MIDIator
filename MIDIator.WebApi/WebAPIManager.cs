using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using Anshul.Utilities;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Client;
using MIDIator.Engine;
using MIDIator.Json;
using Newtonsoft.Json;
using Refigure;
using Sanford.Multimedia.Midi;
using Serilog;

namespace MIDIator.Web
{
	public class WebAPIManager
	{
		private HubConnection HubConnection { get; set; }

        private VirtualMIDIManager VirtualMIDIManager { get; set; }
        
        private IDisposable ShutdownSub { get; set; }

        public void InitializeWebAPI(VirtualMIDIManager virtualMIDIManager, Action onShutdown)
		{
			var settings = SerializerSettings.DefaultSettings;
			settings.ContractResolver = new SignalRContractResolver(settings.ContractResolver);
			settings.TypeNameHandling = TypeNameHandling.Auto;
			var serializer = JsonSerializer.Create(settings);
			GlobalHost.DependencyResolver.Register(typeof(JsonSerializer), () => serializer);
			VirtualMIDIManager = virtualMIDIManager;
            
			//start signalr hubs
			StartSignalRHubs();
            //initialize managers
			InitMIDIManager();
            AdminManager.Instantiate();

            ShutdownSub = AdminManager.Instance.OnShutdown.Subscribe(s => onShutdown());
        }

		private void InitMIDIManager()
		{
			MIDIManager.Instantiate(new MIDIDeviceService(), VirtualMIDIManager);

		    if (string.IsNullOrEmpty(Config.Get("WebAPI.ProfileFile")) || !File.Exists(Config.Get("WebAPI.ProfileFile")))
		    {
                //set initial profile if none is found
                MIDIManager.Instance.SetProfile(new Profile()
                {
                    Name = "DefaultProfile",
                    Transformations = new List<Transformation>(),
                    VirtualLoopbackDevices = new BetterList<VirtualLoopbackDevice>()
                });
            }
		    else
		    {
		        var virtualLoopbackDevices = new BetterList<VirtualLoopbackDevice>();

                var subscription = MIDIManager.Instance.VirtualMIDIManager.VirtualDeviceAdd.Subscribe(
                   device =>
                   {
                       if (device is VirtualLoopbackDevice)
                           virtualLoopbackDevices.Add((VirtualLoopbackDevice)device);
                   });

                var profile = JsonConvert.DeserializeObject<Profile>(File.ReadAllText(Config.Get("WebAPI.ProfileFile")));
                subscription.Dispose();
		        profile.VirtualLoopbackDevices = virtualLoopbackDevices;

                MIDIManager.Instance.SetProfile(profile);
                
            }
		}

		private void StartSignalRHubs()
		{
			HubConnection = new HubConnection(Config.Get("WebAPI.BaseAddress"));
			IHubProxy eventHubProxy = HubConnection.CreateHubProxy("MIDIReaderHub");
			eventHubProxy.On<string, ChannelEvent>("OnEvent",
				(channel, ev) => Log.Information("Event received on {channel} channel - {@ev}", channel, ev));
			HubConnection.Start().Wait();

			eventHubProxy.Invoke("Subscribe", Constants.AdminChannel);
			eventHubProxy.Invoke("Subscribe", Constants.TaskChannel);
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
