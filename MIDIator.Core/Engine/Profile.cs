using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using MIDIator.UIGeneration;
using MIDIator.UIGenerator.Consumables;
using TypeLite;

namespace MIDIator.Engine
{
	/// <summary>
	/// Defines a profile that can be loaded from a persistent store.
	/// </summary>
	[TsClass(Module = "MIDIator.UI")]
	[Ng2Component(typeof(ProfileView))]
	public class Profile : IProfile
	{
		public string Name { get; set; }

		public List<Transformation> Transformations { get; set; }

		public List<VirtualOutputDevice> VirtualOutputDevices { get; set; }
	}
}
