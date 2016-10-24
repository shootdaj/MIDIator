using System.Collections.Generic;
using System.Web.Http;
using Anshul.Utilities;
using MIDIator.Engine;
using MIDIator.Interfaces;
using Sanford.Multimedia.Midi;

namespace MIDIator.Web.Controllers
{
	[RoutePrefix("midi")]
	public class MIDIController : ApiController
	{
		public IMIDIManager MIDIManager { get; set; }

		public MIDIController(/*IMIDIManager midiManager*/)
		{
			MIDIManager = new MIDIManager(); //needs to be injected
		}

		#region Profile

		[HttpGet]
		public Profile Profile()
		{
			var midiDeviceName = "Numark ORBIT";

			return new Profile()
			{
				Name = "DefaultProfile",
				Transformations = new List<Transformation>()
				{
					new Transformation("TouchOSCXForm",
						MIDIManager.GetInputDevice(midiDeviceName),
						MIDIManager.GetOutputDevice(midiDeviceName),
						new TranslationMap(new Translation(new ChannelMessage(ChannelCommand.NoteOn, 1, 66),
							new ChannelMessage(ChannelCommand.ProgramChange, 1, 23),
							InputMatchFunction.NoteMatch, TranslationFunction.DirectTranslation).Listify()
							))
				}
			};
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
			return MIDIManager.AvailableInputDevices;
		}

		[HttpPost]
		public IMIDIInputDevice GetInputDevice(string name)
		{
			return MIDIManager.GetInputDevice(name);
		}

		[HttpPost]
		public void RemoveInputDevice(string name)
		{
			MIDIManager.RemoveInputDevice(name);
		}

		[HttpPost]
		public void SetTranslationMap(string inputDevice, TranslationMap map)
		{
			MIDIManager.SetTranslationMap(inputDevice, map);
		}

		#endregion

		#region Output Devices

		[HttpGet]
		public IEnumerable<dynamic> AvailableOutputDevices()
		{
			return MIDIManager.AvailableOutputDevices;
		}

		[HttpPost]
		public IMIDIOutputDevice GetOutputDevice(string name)
		{
			return MIDIManager.GetOutputDevice(name);
		}

		[HttpPost]
		public void RemoveOutputDevice(string name)
		{
			MIDIManager.RemoveOutputDevice(name);
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
	}
}
