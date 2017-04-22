using System.Collections.Generic;
using System.Dynamic;
using System.Web.Http;
using Microsoft.AspNet.SignalR;
using MIDIator.Engine;
using MIDIator.Interfaces;
using MIDIator.Web.Hubs;
using Newtonsoft.Json.Linq;
using Sanford.Multimedia.Midi;

namespace MIDIator.Web.Controllers
{
	[RoutePrefix("midi")]
	public class MIDIController : ApiController
	{
        public SignalRService SignalRService { get; set; }
		
		public IMIDIManager MIDIManager { get; set; }

		private IHubContext HubContext => SignalRService.HubContext;

		public MIDIController(/*IMIDIManager midiManager*/)
		{
			MIDIManager = Engine.MIDIManager.Instance; //needs to be injected
            SignalRService = new SignalRService();
        }

		#region Profile

		[HttpGet]
		public Profile Profile()
		{
			return MIDIManager.CurrentProfile;
		}

		[HttpPost]
		public dynamic Profile(JObject inProfile)
		{
			MIDIManager.UpdateProfile(inProfile);
			return MIDIManager.CurrentProfile;
		}

		#endregion

		#region Transformations

		//[HttpPost]
		//public Transformation CreateTransformation(string name, string inputDeviceName, string outputDeviceName, TranslationMap translationMap, bool startDevices)
		//{
		//	return MIDIManager.CreateTransformation(name, inputDeviceName, outputDeviceName, translationMap);
		//}

		//[HttpPost]
		//public void RemoveTransformation(string name)
		//{
		//	MIDIManager.RemoveTransformation(name);
		//}

		#endregion

		#region Input Devices

		[HttpGet]
		public IEnumerable<dynamic> AvailableInputDevices()
		{
			return MIDIManager.MIDIDeviceService.AvailableInputDevices;
		}

		[HttpPost]
		public IMIDIInputDevice GetInputDevice(string name)
		{
			return MIDIManager.MIDIDeviceService.GetInputDevice(name, broadcastAction: payload =>
			{
                HubContext.Clients.Group(Constants.TaskChannel).OnEvent(Constants.TaskChannel, new ChannelEvent
			    {
			        ChannelName = Constants.TaskChannel,
			        Name = "broadcastMidiEvent",
			        Data = payload
			    });

            });
		}

		[HttpPost]
		public void RemoveInputDevice(string name)
		{
			MIDIManager.MIDIDeviceService.RemoveInputDevice(name);
		}

		[HttpPost]
		public void SetTranslationMap(string inputDevice, TranslationMap map)
		{
			MIDIManager.MIDIDeviceService.SetTranslationMap(inputDevice, map);
		}

		[HttpPost]
		public void StartMIDIReader(dynamic inputDeviceName)
		{
			MIDIManager.MIDIDeviceService.StartMIDIReader(inputDeviceName.inputDeviceName.Value as string, args =>
			{
				HubContext.Clients.Group(Constants.TaskChannel).OnEvent(Constants.TaskChannel, new ChannelEvent
				{
					ChannelName = Constants.TaskChannel,
					Name = "midiChannelEvent",
					Data = args.Message
				});
			});
		}

		[HttpPost]
		public void StopMIDIReader(dynamic inputDeviceName)
		{
			MIDIManager.MIDIDeviceService.StopMIDIReader(inputDeviceName.inputDeviceName.Value as string);
		}

        [HttpPost]
        public void SendMessageToOutputDevice(dynamic payload)
        {
            MIDIManager.MIDIDeviceService.GetOutputDevice(payload.outputDeviceName.Value as string)
                .Send(payload.message.ToObject<ChannelMessage>());
        }

        #endregion

        #region Output Devices

        [HttpGet]
		public IEnumerable<dynamic> AvailableOutputDevices()
		{
			return MIDIManager.MIDIDeviceService.AvailableOutputDevices;
		}

		[HttpPost]
		public IMIDIOutputDevice GetOutputDevice(string name)
		{
			return MIDIManager.MIDIDeviceService.GetOutputDevice(name);
		}

		[HttpPost]
		public void RemoveOutputDevice(string name)
		{
			MIDIManager.MIDIDeviceService.RemoveOutputDevice(name);
		}


		#endregion

		#region Translation

		[HttpGet]
		public IEnumerable<string> ChannelCommands()
		{
			return MIDIManager.ChannelCommands();
		}

		[HttpGet]
		public IEnumerable<int> MIDIChannels()
		{
			return MIDIManager.MIDIChannels();
		}

		#endregion

		#region Enums

		[HttpGet]
		public IEnumerable<int> AvailableInputMatchFunctions()
		{
			return MIDIManager.AvailableInputMatchFunctions();
		}

		[HttpGet]
		public IEnumerable<int> AvailableTranslationFunctions()
		{
			return MIDIManager.AvailableTranslationFunctions();
		}

		[HttpGet]
		public IEnumerable<int> AvailableChannelCommands()
		{
			return MIDIManager.AvailableChannelCommands();
		}

		[HttpGet]
		public IEnumerable<int> AvailableMIDIChannels()
		{
			return MIDIManager.AvailableMIDIChannels();
		}

		#endregion
	}
}
