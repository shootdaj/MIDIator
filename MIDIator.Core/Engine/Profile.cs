using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using MIDIator.UIGeneration;
using MIDIator.UIGenerator.Consumables;
using TypeLite;

namespace MIDIator.Engine
{
	/// <summary>
	/// Defines a profile that can be loaded from a persistent store.
	/// </summary>
	[TsClass(Module = "")]
	[Ng2Component(typeof(ProfileView), componentCodeTemplate: typeof(ProfileComponentCode))]//, new[] {typeof(Transformation), typeof(VirtualOutputDevice)})]
	public class Profile : IProfile
	{
		public string Name { get; set; }

		public List<Transformation> Transformations { get; set; }

		public List<VirtualOutputDevice> VirtualOutputDevices { get; set; }

		public void Update(ExpandoObject inProfile, MIDIDeviceService midiDeviceService, VirtualMIDIManager virtualMIDIManager = null)
		{
			dynamic profile = inProfile;

			Name = profile.Name;

			foreach (var transformation in profile.Transformations)
			{
				var matchedTransformations = Transformations.Where(t => t.Name == transformation.Name);

				//TODO: check which transformations are different - currently updating all transformations

				var matchedTransformationsList = matchedTransformations.ToList();
				if (matchedTransformationsList.Any())
				{
					var matchedTransformation = matchedTransformationsList.Single();
					matchedTransformation.Update(transformation, midiDeviceService, virtualMIDIManager);
				}
				//TODO: else create new transformations
			}

			//TODO: delete transformations that dont exist in inProfile
		}
	}
}
