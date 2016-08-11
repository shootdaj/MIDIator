﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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

		public static IList<MIDIInputDevice> DevicesInUse { get; } = new List<MIDIInputDevice>();

        public static MIDIInputDevice GetInputDevice(int deviceID, bool failSilently = false)
        {
	        if (DevicesInUse != null && DevicesInUse.Any(device => device.DeviceID == deviceID))
		        return DevicesInUse.First(device => device.DeviceID == deviceID);
	        else
	        {
				if (FreeDevices.Any(d => d.DeviceID == deviceID))
				{
					return CreateInputDevice(deviceID);
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

	    public static MIDIInputDevice GetInputDevice(string name, bool failSilently = false)
	    {
		    if (DevicesInUse != null && DevicesInUse.Any(d => d.Name == name))
			    return DevicesInUse.First(d => d.Name == name);
		    else
		    {
			    if (FreeDevices.Any(d => d.Name == name))
			    {
					var deviceID = FreeDevices.Single(d => d.Name == name).DeviceID;
				    return CreateInputDevice(deviceID);
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

	    public static MIDIInputDevice CreateInputDevice(int deviceID)
		{
			var device = new MIDIInputDevice(deviceID);
			DevicesInUse.Add(device);
			return device;
		}

		public static void RemoveDevice(MIDIInputDevice inputDevice)
		{
			DevicesInUse.Remove(inputDevice);
			inputDevice.Dispose();
		}

		
	}
}
