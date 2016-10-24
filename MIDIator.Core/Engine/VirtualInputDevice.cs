using System;

namespace MIDIator.Engine
{	
	public class VirtualInputDevice : VirtualDevice
	{
		public VirtualInputDevice(string name, ref Guid manufacturerID, ref Guid productID, bool prefixName = true)
			: base(name, ref manufacturerID, ref productID, VirtualDeviceType.Input, prefixName)
		{
		}
	}
}