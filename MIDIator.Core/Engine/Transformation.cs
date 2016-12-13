using System.Dynamic;
using MIDIator.Interfaces;
using MIDIator.UIGeneration;
using MIDIator.UIGenerator.Consumables;
using Newtonsoft.Json;
using TypeLite;

namespace MIDIator.Engine
{
	[TsClass(Module = "")]
	[Ng2Component(componentCodeTemplate: typeof(TransformationComponentCode))]
	public class Transformation
	{
		public string Name { get; set; }

		private string ForwardActionName { get; } = "Forward";

		public IMIDIInputDevice InputDevice { get; set; }

		public IMIDIOutputDevice OutputDevice { get; set; }

		public ITranslationMap TranslationMap
		{
			get { return InputDevice.TranslationMap; }
			set { InputDevice.TranslationMap = value; }
		}

		public bool LinkedOutputVirtualDevice { get; set; } = true;

		public Transformation(string name, IMIDIInputDevice inputDevice, IMIDIOutputDevice outputDevice, ITranslationMap translationMap)
		{
			Name = name;
			InputDevice = inputDevice;
			OutputDevice = outputDevice;
			InputDevice.TranslationMap = translationMap;
			InputDevice.AddChannelMessageAction(new ChannelMessageAction(message => true, OutputDevice.Send, ForwardActionName));
			InputDevice.Start();
		}

		public Transformation(string name, MIDIInputDevice inputDevice, MIDIOutputDevice outputDevice, ITranslationMap translationMap) 
			: this(name, (IMIDIInputDevice) inputDevice, (IMIDIOutputDevice)outputDevice, translationMap)
		{
			
		}

		public void Dispose()
		{
			InputDevice.RemoveChannelMessageAction(ForwardActionName);
		}

		public void Update(dynamic transformation, MIDIDeviceService midiDeviceService, VirtualMIDIManager virtualMIDIManager = null)
		{
			InputDevice.Stop();
			InputDevice.TranslationMap = null;
			InputDevice.RemoveChannelMessageAction(ForwardActionName);
			InputDevice = midiDeviceService.GetInputDevice((string)transformation.InputDevice.Name, virtualMIDIManager: virtualMIDIManager);
			LinkedOutputVirtualDevice = transformation.LinkedOutputVirtualDevice;
			OutputDevice = virtualMIDIManager != null && LinkedOutputVirtualDevice// && !virtualMIDIManager.DoesDeviceExist(InputDevice.Name) -- this was to restrict virtual devices to only be created on real devices, not other virtual devices.
				? midiDeviceService.GetOutputDevice(Extensions.GetVirtualDeviceName(InputDevice.Name))
				: midiDeviceService.GetOutputDevice((string) transformation.OutputDevice.Name);
			TranslationMap = ((ExpandoObject) transformation.TranslationMap).ConvertAsJsonTo<TranslationMap>();
			InputDevice.AddChannelMessageAction(new ChannelMessageAction(message => true, OutputDevice.Send, ForwardActionName));
			InputDevice.Start();
		}
	}
}
