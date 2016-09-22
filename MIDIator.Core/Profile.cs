using System.Collections.Generic;

namespace MIDIator
{
	/// <summary>
	/// Defines a profile that can be loaded from a persistent store.
	/// </summary>
	public class Profile
	{
		public string Name { get; set; }
		
		public List<Transformation> Transformations { get; set; }

		public List<VirtualOutputDevice> VirtualOutputDevices { get; set; }
	}
}
