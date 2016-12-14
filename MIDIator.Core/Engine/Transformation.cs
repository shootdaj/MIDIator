using System;
using System.ComponentModel;
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

		public bool LinkedOutputVirtualDevice { get; set; } = false;

		public Transformation(string name, IMIDIInputDevice inputDevice, IMIDIOutputDevice outputDevice, ITranslationMap translationMap)
		{
			InputDevice = inputDevice;
			OutputDevice = outputDevice;
			InitCore(name, translationMap);
		}

		public Transformation(string name, MIDIInputDevice inputDevice, MIDIOutputDevice outputDevice, ITranslationMap translationMap) 
			: this(name, (IMIDIInputDevice) inputDevice, (IMIDIOutputDevice)outputDevice, translationMap)
		{
			
		}

		/// <summary>
		/// Creates a transformation
		/// </summary>
		/// <param name="name"></param>
		/// <param name="inputDeviceName"></param>
		/// <param name="outputDeviceName"></param>
		/// <param name="midiDeviceService"></param>
		/// <param name="translationMap"></param>
		/// <param name="linkedVirtualOutputDevice">If true, outputDeviceName is ignored and a new virtual output device is created from the given input device</param>
		/// <param name="virtualMIDIManager"></param>
		public Transformation(string name, string inputDeviceName, string outputDeviceName, ITranslationMap translationMap, bool linkedVirtualOutputDevice, MIDIDeviceService midiDeviceService, VirtualMIDIManager virtualMIDIManager)
		{
			InitFromServices(name, inputDeviceName, outputDeviceName, translationMap, midiDeviceService, linkedVirtualOutputDevice, virtualMIDIManager);
		}

		public Transformation(string name, dynamic transformation, MIDIDeviceService midiDeviceService,
			VirtualMIDIManager virtualMIDIManager)
		{
			InitFromDynamicTransformation(name, transformation, midiDeviceService, virtualMIDIManager);
		}

		public void Dispose()
		{
			InputDevice.RemoveChannelMessageAction(ForwardActionName);
		}

		public void Update(dynamic transformation, MIDIDeviceService midiDeviceService, VirtualMIDIManager virtualMIDIManager)
		{
			InputDevice.Stop();
			InputDevice.TranslationMap = null;
			InputDevice.RemoveChannelMessageAction(ForwardActionName);
			InitFromDynamicTransformation(Name, transformation, midiDeviceService, virtualMIDIManager);
		}

		private void InitFromDynamicTransformation(string name, dynamic transformation, MIDIDeviceService midiDeviceService, VirtualMIDIManager virtualMIDIManager)
		{
			InitFromServices(name, (string) transformation.InputDevice.Name, (string) transformation.OutputDevice.Name,
				((ExpandoObject) transformation.TranslationMap).ConvertAsJsonTo<TranslationMap>(), midiDeviceService,
				transformation.LinkedOutputVirtualDevice, transformation.LinkedOutputVirtualDevice ? virtualMIDIManager : null);
		}

		private void InitFromServices(string name, string inputDeviceName, string outputDeviceName, ITranslationMap translationMap, MIDIDeviceService midiDeviceService, bool linkedOutputVirtualDevice, VirtualMIDIManager virtualMIDIManager = null)
		{
			if (linkedOutputVirtualDevice && virtualMIDIManager == null)
				throw new ArgumentException($"If {nameof(linkedOutputVirtualDevice)} is true, {nameof(virtualMIDIManager)} must be provided."); 

			if (!linkedOutputVirtualDevice && virtualMIDIManager != null)
				throw new WarningException($"{nameof(virtualMIDIManager)} passed in with {nameof(linkedOutputVirtualDevice)} set to false, therefore {nameof(virtualMIDIManager)} will be ignored. Did you mean to send it as true?");

			LinkedOutputVirtualDevice = linkedOutputVirtualDevice;
			InputDevice = LinkedOutputVirtualDevice
				? midiDeviceService.GetInputDevice(inputDeviceName, virtualMIDIManager: virtualMIDIManager)
				: midiDeviceService.GetInputDevice(inputDeviceName);
			OutputDevice = virtualMIDIManager != null && LinkedOutputVirtualDevice
				// && !virtualMIDIManager.DoesDeviceExist(InputDevice.Name) -- this was to restrict virtual devices to only be created on real devices, not other virtual devices.
				? midiDeviceService.GetOutputDevice(Extensions.GetVirtualDeviceName(InputDevice.Name))
				: midiDeviceService.GetOutputDevice(outputDeviceName);

			InitCore(name, translationMap);
		}

		private void InitCore(string name, ITranslationMap translationMap)
		{
			Name = name;
			TranslationMap = translationMap;
			InputDevice.AddChannelMessageAction(new ChannelMessageAction(message => true, OutputDevice.Send, ForwardActionName));
			InputDevice.Start();
		}
	}
}

