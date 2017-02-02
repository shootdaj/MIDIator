using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading;
using MIDIator.Engine;
using MIDIator.Interfaces;
using Refigure;
using Sanford.Multimedia.Midi;

namespace MIDIator.Services
{
	public class MIDIDeviceService : IDisposable
	{
		#region Internals

		public IList<IMIDIInputDevice> InputDevicesInUse { get; } = new List<IMIDIInputDevice>();

		public IList<IMIDIOutputDevice> OutputDevicesInUse { get; } = new List<IMIDIOutputDevice>();

		#endregion

		#region Input Devices

		public int InputDeviceCount => InputDevice.DeviceCount;

		public List<dynamic> AvailableInputDevices => AvailableInputDevicesEnumerable.ToList();


		public IEnumerable<dynamic> AvailableInputDevicesEnumerable
		{
			get
			{
				for (var i = 0; i < InputDevice.DeviceCount; i++)
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
						throw new Exception($"No input device with ID {deviceID} found.");
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
					if (failSilently)
						return null;
					else
						throw new Exception($"No input device with name {name} found.");
				}
			}
		}

		/// <summary>
		/// Creates and returns a MIDI Input Device
		/// </summary>
		/// <param name="deviceID"></param>
		/// <param name="translationMap"></param>
		/// <returns></returns>
		private MIDIInputDevice CreateInputDevice(int deviceID, ITranslationMap translationMap = null)
		{
			var device = new MIDIInputDevice(deviceID, translationMap);
			InputDevicesInUse.Add(device);

			//if (virtualMIDIManager != null)
			//{
			//	//only create new device if device doesn't already exist. it may already exist if the user had selected this device before and came back to it after changing it.
			//	if (!virtualMIDIManager.DoesDeviceExist(GetVirtualDeviceName(device.Name)))
			//	{
			//		CreateVirtualOutputDevice(virtualMIDIManager, device);
			//	}
			//}

			return device;
		}


		public void CreateVirtualOutputDeviceForInputDevice(IMIDIInputDevice device, VirtualMIDIManager virtualMIDIManager, Profile profile)
		{
			if (!virtualMIDIManager.DoesDeviceExist(GetVirtualDeviceName(device.Name)))
			{
				var virtualDevice = virtualMIDIManager.CreateVirtualDevice(GetVirtualDeviceName(device.Name), Guid.NewGuid(), Guid.NewGuid(),
					VirtualDeviceType.Loopback);

                profile.VirtualLoopbackDevices.Add((VirtualLoopbackDevice)virtualDevice);

                //virtual device creation takes some time
                Thread.Sleep(Config.GetAsInt("Core.VirtualDeviceDelay"));
			}
		}

		private string GetVirtualDeviceName(string deviceName)
		{
			return Extensions.GetVirtualDeviceName(deviceName);
		}

		public void RemoveInputDevice(string name)
		{
			RemoveInputDevice(GetInputDevice(name));
		}

		public void RemoveInputDevice(IMIDIInputDevice inputDevice, VirtualMIDIManager virtualMIDIManager = null)
		{
			var deviceName = inputDevice.Name;
			InputDevicesInUse.Remove(inputDevice);
			((IDisposable)inputDevice).Dispose();
			virtualMIDIManager?.RemoveVirtualDevice(GetVirtualDeviceName(deviceName));
		}

		public void SetTranslationMap(IMIDIInputDevice inputDevice, ITranslationMap map)
		{
			inputDevice.TranslationMap = map;
		}

		public void SetTranslationMap(string inputDevice, ITranslationMap map)
		{
			GetInputDevice(inputDevice).TranslationMap = map;
		}

		public void StartMIDIReader(string deviceName, Action<ChannelMessageEventArgs> messageAction)
		{
			GetInputDevice(deviceName).StartMIDIReader(messageAction);
		}

		public void StopMIDIReader(string deviceName)
		{
			GetInputDevice(deviceName).StopMIDIReader();
		}

		#endregion

		#region Output Devices

		public List<dynamic> AvailableOutputDevices => AvailableOutputDevicesEnumerable.ToList();

		public IEnumerable<dynamic> AvailableOutputDevicesEnumerable
		{
			get
			{
				for (var i = 0; i < OutputDeviceBase.DeviceCount; i++)
				{
					dynamic returnValue = new ExpandoObject();
					returnValue.Name = OutputDeviceBase.GetDeviceCapabilities(i).name;
					returnValue.DriverVersion = OutputDeviceBase.GetDeviceCapabilities(i).driverVersion;
					returnValue.MID = OutputDeviceBase.GetDeviceCapabilities(i).mid;
					returnValue.PID = OutputDeviceBase.GetDeviceCapabilities(i).pid;
					returnValue.Support = OutputDeviceBase.GetDeviceCapabilities(i).support;
					returnValue.DeviceID = i;
					//returnValue.Technology = OutputDeviceBase.GetDeviceCapabilities(i).technology;
					//returnValue.Voices = OutputDeviceBase.GetDeviceCapabilities(i).voices;
					//returnValue.Notes = OutputDeviceBase.GetDeviceCapabilities(i).notes;
					//returnValue.ChannelMask = OutputDeviceBase.GetDeviceCapabilities(i).channelMask;
					yield return returnValue;
				}
			}
		}

		public IMIDIOutputDevice GetOutputDevice(int deviceID, bool failSilently = false)
		{
			if (OutputDevicesInUse != null && OutputDevicesInUse.Any(device => device.DeviceID == deviceID))
				return OutputDevicesInUse.First(device => device.DeviceID == deviceID);
			else
			{
				if (AvailableOutputDevices.Any(d => d.DeviceID == deviceID))
				{
					return CreateOutputDevice(deviceID);
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

		public void Dispose()
		{
			InputDevicesInUse.ToList().ForEach(device => RemoveInputDevice(device));
			OutputDevicesInUse.ToList().ForEach(RemoveOutputDevice);
		}
	}
}
