using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sanford.Multimedia.Midi;

namespace MIDIator
{
	public static class MIDIManager
	{
		public static IEnumerable<dynamic> AvailableDevices
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
		

		public static IList<InputDevice> DevicesInUse { get; } = new List<InputDevice>();

		public static void AddChannelRecievedAction(int deviceID, EventHandler<ChannelMessageEventArgs> action)
		{
			var device = GetNewOrExistingDevice(deviceID);
			device.ChannelMessageReceived += action;
		}

		public static void StartRecording(int deviceID)
		{
			if (DeviceExists(deviceID))
			{
				DevicesInUse.Single(device => device.DeviceID == deviceID).StartRecording();
			}
		}

		public static void StopRecording(int deviceID)
		{
			if (DeviceExists(deviceID))
			{
				DevicesInUse.Single(device => device.DeviceID == deviceID).StopRecording();
			}
		}

		private static InputDevice GetNewOrExistingDevice(int deviceID)
		{
			//new device
			if (!DeviceExists(deviceID))
			{
				var device = new InputDevice(deviceID);
                DevicesInUse.Add(device);
				return device;
			}
			//existing device
			else
			{
				return DevicesInUse.Single(device => device.DeviceID == deviceID);
			}
		}

		private static bool DeviceExists(int deviceID)
		{
			return DevicesInUse.Select(x => x.DeviceID).Contains(deviceID);
		}
	}
}
