using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sanford.Multimedia.Midi;

namespace MIDIator
{
	public class MIDIManager
	{
		public static IEnumerable<dynamic> Devices
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
					yield return returnValue;
				}
			}
		}
	}
}
