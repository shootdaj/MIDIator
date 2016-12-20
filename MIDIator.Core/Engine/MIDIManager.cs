using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using MIDIator.Interfaces;
using Sanford.Multimedia.Midi;

namespace MIDIator.Engine
{
	public class MIDIManager : IMIDIManager, IDisposable
	{
		#region Singleton

		public static void Instantiate(MIDIDeviceService midiDeviceService, VirtualMIDIManager virtualMIDIManager)
		{
			Instance = new MIDIManager(midiDeviceService, virtualMIDIManager);
		}

		public static MIDIManager Instance { get; private set; }

		#endregion

		#region Internals

		public MIDIDeviceService MIDIDeviceService { get; set; }

		public VirtualMIDIManager VirtualMIDIManager { get; set; }

		public MIDIManager(MIDIDeviceService midiDeviceService, VirtualMIDIManager virtualMIDIManager = null)
		{
			MIDIDeviceService = midiDeviceService;
			VirtualMIDIManager = virtualMIDIManager;
		}

		public void Dispose()
		{
			MIDIDeviceService.Dispose();
		}

		#endregion

		#region Profile

		public void SetProfile(Profile profile)
		{
			CurrentProfile = profile;
		}

		public Profile CurrentProfile { get; private set; }

		public void UpdateProfile(ExpandoObject profile)
		{
			CurrentProfile.Update(profile, MIDIDeviceService, VirtualMIDIManager);
		}

		#endregion

		#region Transformations

		public Transformation CreateTransformation(string name, IMIDIInputDevice inputDevice, IMIDIOutputDevice outputDevice, TranslationMap translationMap)
		{
			var transformation = new Transformation(name, inputDevice, outputDevice, translationMap);
			((List<Transformation>)CurrentProfile.Transformations).Add(transformation);
			return transformation;
		}

		public Transformation CreateTransformation(string name, string inputDeviceName, string outputDeviceName,
			TranslationMap translationMap)
		{
			return CreateTransformation(name, MIDIDeviceService.GetInputDevice(inputDeviceName), MIDIDeviceService.GetOutputDevice(outputDeviceName), translationMap);
		}

		public void RemoveTransformation(string name)
		{
			CurrentProfile.Transformations.Where(t => t.Name == name).ToList().ForEach(t => t.Dispose());
			((List<Transformation>)CurrentProfile.Transformations).RemoveAll(t => t.Name == name);
		}

		#endregion

		#region Translations

		public IEnumerable<string> ChannelCommands()
		{
			return Enum.GetNames(typeof(ChannelCommand));
		}

		public IEnumerable<int> MIDIChannels()
		{
			return Enumerable.Range(1, 16).ToArray();
		}

		#endregion

		#region Enums

		public IEnumerable<int> AvailableInputMatchFunctions()
		{
			return Enum.GetValues(typeof(InputMatchFunction)).Cast<int>();
		}

		public IEnumerable<int> AvailableTranslationFunctions()
		{
			return Enum.GetValues(typeof(TranslationFunction)).Cast<int>();
		}

		public IEnumerable<int> AvailableChannelCommands()
		{
			return Enum.GetValues(typeof(ChannelCommand)).Cast<int>();
		}

		public IEnumerable<int> AvailableMIDIChannels()
		{
			return Enumerable.Range(1, 16);
		}


		#endregion
	}
}
