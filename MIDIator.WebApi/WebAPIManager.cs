﻿using System;
using System.Collections.Generic;
using System.Linq;
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

		public void InitializeWebAPI(VirtualMIDIManager virtualMIDIManager)
		{
			var settings = SerializerSettings.DefaultSettings;
			settings.ContractResolver = new SignalRContractResolver(settings.ContractResolver);
			settings.TypeNameHandling = TypeNameHandling.Auto;
			var serializer = JsonSerializer.Create(settings);
			GlobalHost.DependencyResolver.Register(typeof(JsonSerializer), () => serializer);
			VirtualMIDIManager = virtualMIDIManager;

			//start signalr hubs
			StartSignalRHubs();

			//initialize midi manager
			InitMIDIManager();
		}

		private void InitMIDIManager()
		{
			var inputDeviceName = "TouchOSC Bridge"; // "Launchpad";
			var outputDeviceName = string.Empty;
			MIDIManager.Instantiate(new MIDIDeviceService(), VirtualMIDIManager);

			//create translation map - this should be loaded from a service or from disk or something
			var translationMap = new TranslationMap(new Translation(new ChannelMessage(ChannelCommand.NoteOn, 1, 66),
					new ChannelMessage(ChannelCommand.ProgramChange, 1, 23),
					InputMatchFunction.NoteMatch, TranslationFunction.DirectTranslation).Listify()
			);

			//set initial profile - this should be loaded from a service or from disk or something
			MIDIManager.Instance.SetProfile(new Profile()
			{
				Name = "DefaultProfile",
				Transformations = new List<Transformation>()
				{
					new Transformation("TouchOSCXForm", inputDeviceName, string.Empty, translationMap,
						true, MIDIManager.Instance.MIDIDeviceService, MIDIManager.Instance.VirtualMIDIManager)
				}
				//,
				//VirtualOutputDevices = new List<VirtualOutputDevice>()
				//{
				//	(VirtualOutputDevice)MIDIManager.Instance.VirtualMIDIManager.CreateVirtualDevice("TestVirtualDevice", Guid.NewGuid(), Guid.NewGuid(), VirtualDeviceType.Output)
				//}
			});
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
			MIDIManager.Instance.Dispose();
			HubConnection.Stop();
			HubConnection.Dispose();
			HubConnection = null;
		}
	}
}
