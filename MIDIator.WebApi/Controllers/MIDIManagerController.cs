using System.Collections.Generic;
using System.Web.Http;

namespace MIDIator.Web.Controllers
{
	public class MIDIManagerController : ApiController
	{
		[HttpGet]
		public IEnumerable<dynamic> AvailableDevices()
		{
			return MIDIManager.AvailableInputDevices;
		}

		[HttpGet]
		public Profile GetProfile(string name)
		{
			return MIDIManager.CurrentProfile;
		}

		[HttpPost]
		public Profile CreateTransformation(string name, string inputDeviceName, string outputDeviceName, TranslationMap translationMap, bool startDevices)
		{
			MIDIManager.CreateTransformation(name, MIDIManager.GetInputDevice(inputDeviceName),
				MIDIManager.GetOutputDevice(outputDeviceName), translationMap);

			return MIDIManager.CurrentProfile;
		}

		public bool ReadTranslationMap(TranslationMap map)
		{
			return true;
		}
	}
}
