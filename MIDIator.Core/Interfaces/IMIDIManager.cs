using System.Collections.Generic;
using System.Dynamic;
using MIDIator.Engine;
using MIDIator.Services;
using Newtonsoft.Json.Linq;

namespace MIDIator.Interfaces
{
	public interface IMIDIManager
	{
		Profile CurrentProfile { get; }
		MIDIDeviceService MIDIDeviceService { get; set; }
		//Transformation CreateTransformation(string name, IMIDIInputDevice inputDevice, IMIDIOutputDevice outputDevice, TranslationMap translationMap);

		//Transformation CreateTransformation(string name, string inputDeviceName, string outputDeviceName,
		//	TranslationMap translationMap);

		//void RemoveTransformation(string name);
		IEnumerable<string> ChannelCommands();
		IEnumerable<int> MIDIChannels();
		IEnumerable<int> AvailableInputMatchFunctions();
		IEnumerable<int> AvailableTranslationFunctions();
		void UpdateProfile(JObject profile);
		IEnumerable<int> AvailableChannelCommands();
		IEnumerable<int> AvailableMIDIChannels();
	}
}