using System.Collections.Generic;

namespace MIDIator.Engine
{
	public interface IProfile
	{
		string Name { get; set; }
		List<Transformation> Transformations { get; set; }
		List<VirtualOutputDevice> VirtualOutputDevices { get; set; }
	}
}