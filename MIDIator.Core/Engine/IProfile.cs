using System.Collections.Generic;
using Anshul.Utilities;

namespace MIDIator.Engine
{
	public interface IProfile
	{
		string Name { get; set; }
		List<Transformation> Transformations { get; set; }
		BetterList<VirtualLoopbackDevice> VirtualLoopbackDevices { get; set; }
	}
}