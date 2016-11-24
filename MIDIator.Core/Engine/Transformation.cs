using System;
using System.Dynamic;
using System.Runtime.Serialization;
using MIDIator.Interfaces;
using MIDIator.Json;
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

		public Transformation(string name, IMIDIInputDevice inputDevice, IMIDIOutputDevice outputDevice, ITranslationMap translationMap)
		{
			Name = name;
			InputDevice = inputDevice;
			OutputDevice = outputDevice;
			InputDevice.TranslationMap = translationMap;
			InputDevice.AddChannelMessageAction(new ChannelMessageAction(message => true, OutputDevice.Send, ForwardActionName));
		}

		[JsonConstructor]
		public Transformation(string name, MIDIInputDevice inputDevice, MIDIOutputDevice outputDevice, TranslationMap translationMap)
		{
			Name = name;
			InputDevice = MIDIManager.Instance.MIDIDeviceService.GetInputDevice(inputDevice.Name);
			OutputDevice = MIDIManager.Instance.MIDIDeviceService.GetOutputDevice(outputDevice.Name);
			InputDevice.TranslationMap = translationMap;
		}

		public void Dispose()
		{
			InputDevice.RemoveChannelMessageAction(ForwardActionName);
		}

		public void Update(dynamic transformation, MIDIDeviceService midiDeviceService)
		{
			InputDevice = midiDeviceService.GetInputDevice((string)transformation.InputDevice.Name);
			OutputDevice = midiDeviceService.GetOutputDevice((string)transformation.OutputDevice.Name);
			TranslationMap = ((ExpandoObject) transformation.TranslationMap).ConvertTo<TranslationMap>();
		}
	}
}
