using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using MIDIator.Interfaces;
using Sanford.Multimedia.Midi;

namespace MIDIator.Engine
{
	public class MIDIManager : IMIDIManager
	{
		#region Internals

		public IList<IMIDIInputDevice> InputDevicesInUse { get; } = new List<IMIDIInputDevice>();

		public IList<IMIDIOutputDevice> OutputDevicesInUse { get; } = new List<IMIDIOutputDevice>();



		#endregion

		#region Profile

		public Profile CurrentProfile { get; }

		#endregion

		#region Transformations

		public Transformation CreateTransformation(string name, IMIDIInputDevice inputDevice, IMIDIOutputDevice outputDevice, TranslationMap translationMap)
		{
			var transformation = new Transformation(name, inputDevice, outputDevice, translationMap);
			CurrentProfile.Transformations.Add(transformation);
			return transformation;
		}

		public Transformation CreateTransformation(string name, string inputDeviceName, string outputDeviceName,
			TranslationMap translationMap)
		{
			return CreateTransformation(name, GetInputDevice(inputDeviceName), GetOutputDevice(outputDeviceName), translationMap);
		}

		public void RemoveTransformation(string name)
		{
			CurrentProfile.Transformations.Where(t => t.Name == name).ToList().ForEach(t => t.Dispose());
			CurrentProfile.Transformations.RemoveAll(t => t.Name == name);
		}

		#endregion

		#region Input Devices

		public int InputDeviceCount => InputDevice.DeviceCount;

		public List<dynamic> AvailableInputDevices => AvailableInputDevicesEnumerable.ToList();

		public IEnumerable<dynamic> AvailableInputDevicesEnumerable
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

		public IMIDIInputDevice GetInputDevice(int deviceID, ITranslationMap translationMap = null, bool failSilently = false)
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

		public IMIDIInputDevice GetInputDevice(string name, ITranslationMap translationMap = null, bool failSilently = false)
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

					AvailableInputDevices.Select(x => x.Name).ToList().ForEach(y => Debug.Print(y));

					if (failSilently)
						return null;
					else
						throw new Exception($"No device with name {name} found.");
				}
			}
		}

		private MIDIInputDevice CreateInputDevice(int deviceID, ITranslationMap translationMap = null)
		{
			var device = new MIDIInputDevice(deviceID, translationMap);
			InputDevicesInUse.Add(device);
			return device;
		}

		public void RemoveInputDevice(string name)
		{
			RemoveInputDevice(GetInputDevice(name));
		}

		public void RemoveInputDevice(IMIDIInputDevice inputDevice)
		{
			InputDevicesInUse.Remove(inputDevice);
			((IDisposable)inputDevice).Dispose();
		}

		public void SetTranslationMap(IMIDIInputDevice inputDevice, ITranslationMap map)
		{
			inputDevice.TranslationMap = map;
		}

		public void SetTranslationMap(string inputDevice, ITranslationMap map)
		{
			GetInputDevice(inputDevice).TranslationMap = map;
		}

		#endregion

		#region Output Devices

		public List<dynamic> AvailableOutputDevices => AvailableOutputDevicesEnumerable.ToList();

		public IEnumerable<dynamic> AvailableOutputDevicesEnumerable
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

		public IMIDIOutputDevice GetOutputDevice(string name, bool failSilently = false)
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
		private MIDIOutputDevice CreateOutputDevice(int deviceID)
		{
			var device = new MIDIOutputDevice(deviceID);
			OutputDevicesInUse.Add(device);
			return device;
		}

		public void RemoveOutputDevice(IMIDIOutputDevice device)
		{
			OutputDevicesInUse.Remove(device);
			((IDisposable)device).Dispose();
		}

		public void RemoveOutputDevice(string name)
		{
			RemoveOutputDevice(GetOutputDevice(name));
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
	}
}
