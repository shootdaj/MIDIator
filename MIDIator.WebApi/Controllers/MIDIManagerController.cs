using System.Collections.Generic;
using System.Web.Http;

namespace MIDIator.Web.Controllers
{
	public class MIDIManagerController : ApiController
	{
		#region Profile

		[HttpGet]
		public Profile GetProfile(string name)
		{
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
		public static void RemoveTransformation(string name)
		{
			MIDIManager.RemoveTransformation(name);
		}

		#endregion

		#region Input Devices

		[HttpGet]
		public static IEnumerable<dynamic> AvailableInputDevices()
		{
			return MIDIManager.AvailableInputDevices;
		}

		[HttpPost]
		public static IMIDIInputDevice GetInputDevice(string name)
		{
			return MIDIManager.GetInputDevice(name);
		}

		[HttpPost]
		public static void RemoveInputDevice(string name)
		{
			MIDIManager.RemoveInputDevice(name);
		}

		[HttpPost]
		public static void SetTranslationMap(string inputDevice, TranslationMap map)
		{
			MIDIManager.SetTranslationMap(inputDevice, map);
		}

		#endregion

		#region Output Devices

		[HttpGet]
		public static IEnumerable<dynamic> AvailableOutputDevices()
		{
			return MIDIManager.AvailableOutputDevices;
		}

		[HttpPost]
		public static IMIDIOutputDevice GetOutputDevice(string name)
		{
			return MIDIManager.GetOutputDevice(name);
		}

		[HttpPost]
		public static void RemoveOutputDevice(string name)
		{
			MIDIManager.RemoveOutputDevice(name);
		}


		#endregion

		//public bool ReadTranslationMap(TranslationMap map)
		//{
		//	return true;
		//}
	}
}
