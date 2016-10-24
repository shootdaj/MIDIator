using System;
using TypeLite;

namespace MIDIator.Engine
{
	[TsClass(Module = "MIDIator.UI")]
	public class VirtualOutputDevice : VirtualDevice
	{
		public VirtualOutputDevice(string name, ref Guid manufacturerID, ref Guid productID, bool prefixName = true) 
			: base(name, ref manufacturerID, ref productID, VirtualDeviceType.Output, prefixName)
		{
		}
	}
}