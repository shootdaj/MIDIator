using System.Collections.Generic;
using MIDIator.Engine;

namespace MIDIator.Interfaces
{
	public interface IMIDIManager
	{
		IList<IMIDIInputDevice> InputDevicesInUse { get; }
		IList<IMIDIOutputDevice> OutputDevicesInUse { get; }
		Profile CurrentProfile { get; }
		int InputDeviceCount { get; }
		List<object> AvailableInputDevices { get; }
		IEnumerable<object> AvailableInputDevicesEnumerable { get; }
		List<object> AvailableOutputDevices { get; }
		IEnumerable<object> AvailableOutputDevicesEnumerable { get; }
		Transformation CreateTransformation(string name, IMIDIInputDevice inputDevice, IMIDIOutputDevice outputDevice, TranslationMap translationMap);

		Transformation CreateTransformation(string name, string inputDeviceName, string outputDeviceName,
			TranslationMap translationMap);

		void RemoveTransformation(string name);
		IMIDIInputDevice GetInputDevice(int deviceID, ITranslationMap translationMap = null, bool failSilently = false);
		IMIDIInputDevice GetInputDevice(string name, ITranslationMap translationMap = null, bool failSilently = false);
		void RemoveInputDevice(string name);
		void RemoveInputDevice(IMIDIInputDevice inputDevice);
		void SetTranslationMap(IMIDIInputDevice inputDevice, ITranslationMap map);
		void SetTranslationMap(string inputDevice, ITranslationMap map);
		IMIDIOutputDevice GetOutputDevice(string name, bool failSilently = false);
		void RemoveOutputDevice(IMIDIOutputDevice device);
		void RemoveOutputDevice(string name);
		IEnumerable<string> ChannelCommands();
		IEnumerable<int> MIDIChannels();
	}
}