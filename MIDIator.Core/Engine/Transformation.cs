using System;
using System.ComponentModel;
using System.Dynamic;
using MIDIator.Interfaces;
using MIDIator.Services;
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
		public Guid ID { get; set; }

		public string Name { get; set; }

		public IMIDIInputDevice InputDevice { get; set; }

		public IMIDIOutputDevice OutputDevice { get; set; }

		public bool Enabled { get; set; } = true;

		public bool Collapsed { get; set; } = false;

		public bool TranslationsCollapsed { get; set; } = false;

		public ITranslationMap TranslationMap
		{
			get { return InputDevice.TranslationMap; }
			set { InputDevice.TranslationMap = value; }
		}

		public bool LinkedOutputVirtualDevice { get; set; } = false;

		/// <summary>
		/// Creates a transformation from services
		/// </summary>
		/// <param name="name"></param>
		/// <param name="outputDevice"></param>
		/// <param name="translationMap"></param>
		/// <param name="linkedVirtualOutputDevice">If true, outputDeviceName is ignored and a new virtual output device is created from the given input device</param>
		/// <param name="inputDevice"></param>
		[JsonConstructor]
		public Transformation(string name, IMIDIInputDevice inputDevice, IMIDIOutputDevice outputDevice, ITranslationMap translationMap, bool linkedVirtualOutputDevice, bool enabled = true, bool collapsed = false, bool translationsCollapsed = false, Guid? id = null)
		{
			InitFromServices(name, inputDevice, outputDevice, translationMap, linkedVirtualOutputDevice, enabled, collapsed, translationsCollapsed, id);
		}

		public Transformation(string name, dynamic transformation, IMIDIInputDevice inputDevice, IMIDIOutputDevice outputDevice, Guid? id = null)
		{
			InitFromDynamicTransformation(name, transformation, inputDevice, outputDevice, id);
		}

		public void Dispose()
		{
			InputDevice.RemoveChannelMessageAction("SendToOutput");
		}

		public void Update(dynamic transformation, IMIDIInputDevice inputDevice, IMIDIOutputDevice outputDevice, Guid? id = null)
		{
			InputDevice.Stop();
			InputDevice.TranslationMap = null;
			InputDevice.RemoveChannelMessageAction("SendToOutput");
			InitFromDynamicTransformation(Name, transformation, inputDevice, outputDevice, id);
		}

		private void InitFromDynamicTransformation(string name, dynamic transformation, IMIDIInputDevice inputDevice, IMIDIOutputDevice outputDevice, Guid? id = null)
		{
			InitFromServices(name, inputDevice, outputDevice,
				((ExpandoObject)transformation.TranslationMap).ConvertAsJsonTo<TranslationMap>(), transformation.LinkedOutputVirtualDevice, id: id);
		}

		private void InitFromServices(string name, IMIDIInputDevice midiInputDevice, IMIDIOutputDevice midiOutputDevice, ITranslationMap translationMap, bool linkedOutputVirtualDevice, bool enabled = true, bool collapsed = false, bool translationsCollapsed = false, Guid? id = null)
		{
			InitCore(name, translationMap, linkedOutputVirtualDevice, midiInputDevice, midiOutputDevice, enabled, collapsed, translationsCollapsed, id);
		}

		private void InitCore(string name, ITranslationMap translationMap, bool linkedOutputVirtualDevice, IMIDIInputDevice inputDevice, IMIDIOutputDevice outputDevice, bool enabled = true, bool collapsed = false, bool translationsCollapsed = false, Guid? id = null)
		{
			if (id != Guid.Empty && id != null)
				ID = (Guid)id;
			else
				ID = Guid.NewGuid();
			Name = name;
			Enabled = enabled;
			InputDevice = inputDevice;
			OutputDevice = outputDevice;
			TranslationMap = translationMap;
			LinkedOutputVirtualDevice = linkedOutputVirtualDevice;
			Collapsed = collapsed;
			TranslationsCollapsed = translationsCollapsed;
			InputDevice.AddChannelMessageAction(ChannelMessageAction.SendToOutput(OutputDevice));
			if (Enabled)
				InputDevice.Start();
		}
	}
}

