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
        public string Name { get; set; }

        private string ForwardActionName { get; } = "Forward";

        public IMIDIInputDevice InputDevice { get; set; }

        public IMIDIOutputDevice OutputDevice { get; set; }

        public bool Enabled { get; set; } = true;

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
        public Transformation(string name, IMIDIInputDevice inputDevice, IMIDIOutputDevice outputDevice, ITranslationMap translationMap, bool linkedVirtualOutputDevice)
        {
            InitFromServices(name, inputDevice, outputDevice, translationMap, linkedVirtualOutputDevice);
        }

        public Transformation(string name, dynamic transformation, IMIDIInputDevice inputDevice, IMIDIOutputDevice outputDevice)
        {
            InitFromDynamicTransformation(name, transformation, inputDevice, outputDevice);
        }

        public void Dispose()
        {
            InputDevice.RemoveChannelMessageAction(ForwardActionName);
        }

        public void Update(dynamic transformation, IMIDIInputDevice inputDevice, IMIDIOutputDevice outputDevice)
        {
            InputDevice.Stop();
            InputDevice.TranslationMap = null;
            InputDevice.RemoveChannelMessageAction(ForwardActionName);
            InitFromDynamicTransformation(Name, transformation, inputDevice, outputDevice);
        }

        private void InitFromDynamicTransformation(string name, dynamic transformation, IMIDIInputDevice inputDevice, IMIDIOutputDevice outputDevice)
        {
            InitFromServices(name, inputDevice, outputDevice,
                ((ExpandoObject)transformation.TranslationMap).ConvertAsJsonTo<TranslationMap>(), transformation.LinkedOutputVirtualDevice);
        }

        private void InitFromServices(string name, IMIDIInputDevice midiInputDevice, IMIDIOutputDevice midiOutputDevice, ITranslationMap translationMap, bool linkedOutputVirtualDevice)
        {
            InitCore(name, translationMap, linkedOutputVirtualDevice, midiInputDevice, midiOutputDevice);
        }

        private void InitCore(string name, ITranslationMap translationMap, bool linkedOutputVirtualDevice, IMIDIInputDevice inputDevice, IMIDIOutputDevice outputDevice)
        {
            Name = name;
            InputDevice = inputDevice;
            OutputDevice = outputDevice;
            TranslationMap = translationMap;
            LinkedOutputVirtualDevice = linkedOutputVirtualDevice;
            InputDevice.AddChannelMessageAction(new ChannelMessageAction(message => true, OutputDevice.Send, ForwardActionName));
            InputDevice.Start();
        }
    }
}

