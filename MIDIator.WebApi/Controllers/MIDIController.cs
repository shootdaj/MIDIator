using System.Collections.Generic;
using System.Dynamic;
using System.Web.Http;
using MIDIator.Engine;
using MIDIator.Interfaces;

namespace MIDIator.Web.Controllers
{
	[RoutePrefix("midi")]
	public class MIDIController : ApiController
	{
		public IMIDIManager MIDIManager { get; set; }

		public MIDIController(/*IMIDIManager midiManager*/)
		{
			MIDIManager = Engine.MIDIManager.Instance; //needs to be injected
		}

		#region Profile

		[HttpGet]
		public Profile Profile()
		{
			return MIDIManager.CurrentProfile;
		}

		[HttpPost]
		public dynamic Profile(ExpandoObject inProfile)
		{
			MIDIManager.UpdateProfile(inProfile);
			return MIDIManager.CurrentProfile;
		}

		#endregion

		#region Transformations

		[HttpPost]
		public Transformation CreateTransformation(string name, string inputDeviceName, string outputDeviceName, TranslationMap translationMap, bool startDevices)
		{
			return MIDIManager.CreateTransformation(name, inputDeviceName, outputDeviceName, translationMap);
		}

		[HttpPost]
		public void RemoveTransformation(string name)
		{
			MIDIManager.RemoveTransformation(name);
		}

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
			return MIDIManager.MIDIDeviceService.GetInputDevice(name);
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

		#endregion
	}
}
