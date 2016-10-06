using System.Collections.Generic;
using TypeLite;

namespace MIDIator
{
	/// <summary>
	/// Defines a profile that can be loaded from a persistent store.
	/// </summary>
	[TsClass]
	public class Profile
	{
		public string Name { get; set; }
		
		public List<Transformation> Transformations { get; set; }

		public List<VirtualOutputDevice> VirtualOutputDevices { get; set; }
	}
}
