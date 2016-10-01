using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Sanford.Multimedia.Midi;

namespace MIDIator
{
	public static class MIDIManager
	{
		static MIDIManager()
		{
			CurrentProfile = new Profile { Name = "DefaultProfile", }; //later load this from somewhere
		}

		#region Internals

		public static IList<IMIDIInputDevice> InputDevicesInUse { get; } = new List<IMIDIInputDevice>();

		public static IList<IMIDIOutputDevice> OutputDevicesInUse { get; } = new List<IMIDIOutputDevice>();

		private static MIDIInputDevice CreateInputDevice(int deviceID, ITranslationMap translationMap = null)
		{
			var device = new MIDIInputDevice(deviceID, translationMap);
			InputDevicesInUse.Add(device);
			return device;
		}

		private static MIDIOutputDevice CreateOutputDevice(int deviceID)
		{
			var device = new MIDIOutputDevice(deviceID);
			OutputDevicesInUse.Add(device);
			return device;
		}

		#endregion

		#region Profile

		public static Profile CurrentProfile { get; }

		#endregion

		#region Transformations

		public static Transformation CreateTransformation(string name, IMIDIInputDevice inputDevice, IMIDIOutputDevice outputDevice, TranslationMap translationMap)
		{
			var transformation = new Transformation(name, inputDevice, outputDevice, translationMap);
			CurrentProfile.Transformations.Add(transformation);
			return transformation;
		}

		public static Transformation CreateTransformation(string name, string inputDeviceName, string outputDeviceName,
			TranslationMap translationMap)
		{
			return CreateTransformation(name, GetInputDevice(inputDeviceName), GetOutputDevice(outputDeviceName), translationMap);
		}

		public static void RemoveTransformation(string name)
		{
			CurrentProfile.Transformations.Where(t => t.Name == name).ToList().ForEach(t => t.Dispose());
			CurrentProfile.Transformations.RemoveAll(t => t.Name == name);
		}

		#endregion

		#region Input Devices

		public static int InputDeviceCount => InputDevice.DeviceCount;

		public static IEnumerable<dynamic> AvailableInputDevices
		{
			get
			{
				for (int i = 0; i < InputDevice.DeviceCount; i++)
				{
					dynamic returnValue = new ExpandoObject();
					returnValue.Name = InputDevice.GetDeviceCapabilities(i).name;
					returnValue.DriverVersion = InputDevice.GetDeviceCapabilities(i).driverVersion;
					returnValue.MID = InputDevice.GetDeviceCapabilities(i).mid;
					returnValue.PID = InputDevice.GetDeviceCapabilities(i).pid;
					returnValue.Support = InputDevice.GetDeviceCapabilities(i).support;
					returnValue.DeviceID = i;
					yield return returnValue;
				}
			}
		}

		public static IMIDIInputDevice GetInputDevice(int deviceID, ITranslationMap translationMap = null, bool failSilently = false)
		{
			if (InputDevicesInUse != null && InputDevicesInUse.Any(device => device.DeviceID == deviceID))
				return InputDevicesInUse.First(device => device.DeviceID == deviceID);
			else
			{
				if (AvailableInputDevices.Any(d => d.DeviceID == deviceID))
				{
					return CreateInputDevice(deviceID, translationMap);
				}
				else
				{
					if (failSilently)
						return null;
					else
						throw new Exception($"No device with ID {deviceID} found.");
				}

			}
		}

		public static IMIDIInputDevice GetInputDevice(string name, ITranslationMap translationMap = null, bool failSilently = false)
		{
			Func<dynamic, bool> nameMatch = d => d.Name.Equals(name, StringComparison.OrdinalIgnoreCase);
			if (InputDevicesInUse != null && InputDevicesInUse.Any(nameMatch))
				return InputDevicesInUse.First(nameMatch);
			else
			{
				if (AvailableInputDevices.Any(nameMatch))
				{
					var deviceID = AvailableInputDevices.Single(d => d.Name == name).DeviceID;
					return CreateInputDevice(deviceID, translationMap);
				}
				else
				{
					if (failSilently)
						return null;
					else
						throw new Exception($"No device with name {name} found.");
				}
			}
		}

		public static void RemoveInputDevice(string name)
		{
			RemoveInputDevice(GetInputDevice(name));
		}

		public static void RemoveInputDevice(IMIDIInputDevice inputDevice)
		{
			InputDevicesInUse.Remove(inputDevice);
			((IDisposable)inputDevice).Dispose();
		}

		public static void SetTranslationMap(IMIDIInputDevice inputDevice, ITranslationMap map)
		{
			inputDevice.TranslationMap = map;
		}

		public static void SetTranslationMap(string inputDevice, ITranslationMap map)
		{
			GetInputDevice(inputDevice).TranslationMap = map;
		}

		#endregion

		#region Output Devices

		public static IEnumerable<dynamic> AvailableOutputDevices
		{
			get
			{
				for (int i = 0; i < OutputDeviceBase.DeviceCount; i++)
				{
					dynamic returnValue = new ExpandoObject();
					returnValue.Name = OutputDeviceBase.GetDeviceCapabilities(i).name;
					returnValue.DriverVersion = OutputDeviceBase.GetDeviceCapabilities(i).driverVersion;
					returnValue.MID = OutputDeviceBase.GetDeviceCapabilities(i).mid;
					returnValue.PID = OutputDeviceBase.GetDeviceCapabilities(i).pid;
					returnValue.Support = OutputDeviceBase.GetDeviceCapabilities(i).support;
					returnValue.DeviceID = i;
					returnValue.Technology = OutputDeviceBase.GetDeviceCapabilities(i).technology;
					returnValue.Voices = OutputDeviceBase.GetDeviceCapabilities(i).voices;
					returnValue.Notes = OutputDeviceBase.GetDeviceCapabilities(i).notes;
					returnValue.ChannelMask = OutputDeviceBase.GetDeviceCapabilities(i).channelMask;
					yield return returnValue;
				}
			}
		}

		public static IMIDIOutputDevice GetOutputDevice(string name, bool failSilently = false)
		{
			Func<dynamic, bool> nameMatch = d => d.Name.Equals(name, StringComparison.OrdinalIgnoreCase);
			if (OutputDevicesInUse != null && OutputDevicesInUse.Any(nameMatch))
				return OutputDevicesInUse.First(nameMatch);
			else
			{
				if (AvailableOutputDevices.Any(nameMatch))
				{
					var deviceID = AvailableOutputDevices.Single(d => d.Name == name).DeviceID;
					return CreateOutputDevice(deviceID);
				}
				else
				{
					if (failSilently)
						return null;
					else
						throw new Exception($"No device with name {name} found.");
				}
			}
		}

		public static void RemoveOutputDevice(IMIDIOutputDevice device)
		{
			OutputDevicesInUse.Remove(device);
			((IDisposable)device).Dispose();
		}
		
		public static void RemoveOutputDevice(string name)
		{
			RemoveOutputDevice(GetOutputDevice(name));
		}

		#endregion

		#region Translations

		public static IEnumerable<string> ChannelCommands()
		{
			return Enum.GetNames(typeof(ChannelCommand));
		}

		public static IEnumerable<int> MIDIChannels()
		{
			return Enumerable.Range(1, 16).ToArray();
		}

		#endregion
	}
}
