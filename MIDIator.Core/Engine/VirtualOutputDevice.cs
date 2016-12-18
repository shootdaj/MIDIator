using System;
using TypeLite;

namespace MIDIator.Engine
{
	[TsClass(Module = "")]
	public class VirtualOutputDevice : VirtualDevice
	{
		public VirtualOutputDevice(string name, ref Guid manufacturerID, ref Guid productID) 
			: base(name, ref manufacturerID, ref productID, VirtualDeviceType.Output)
		{
		}
	}
}