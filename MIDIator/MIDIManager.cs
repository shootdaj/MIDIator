using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Sanford.Multimedia.Midi;

namespace MIDIator
{
    public static class MIDIManager
	{
        public static int DeviceCount => InputDevice.DeviceCount;

		public static IEnumerable<dynamic> FreeDevices
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

		public static IList<MIDIDevice> DevicesInUse { get; } = new List<MIDIDevice>();

        public static MIDIDevice GetDevice(int deviceID)
        {
            return DevicesInUse != null && DevicesInUse.Any(device => device.DeviceID == deviceID)
                ? DevicesInUse.First(device => device.DeviceID == deviceID)
                : CreateDevice(deviceID);
        }

        public static MIDIDevice CreateDevice(int deviceID)
		{
			var device = new MIDIDevice(deviceID);
			DevicesInUse.Add(device);
			return device;
		}

		public static void RemoveDevice(MIDIDevice device)
		{
			DevicesInUse.Remove(device);
			device.Dispose();
		}
	}
}
