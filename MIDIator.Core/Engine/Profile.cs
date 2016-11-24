using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Runtime.Serialization;
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

		public void Update(ExpandoObject inProfile, MIDIDeviceService midiDeviceService)
		{
			dynamic profile = inProfile;

			Name = profile.Name;

			foreach (var transformation in profile.Transformations)
			{
				var matchedTransformations = Transformations.Where(t => t.Name == transformation.Name);

				var matchedTransformationsList = matchedTransformations.ToList();
				if (matchedTransformationsList.Any())
				{
					var matchedTransformation = matchedTransformationsList.Single();
					matchedTransformation.Update(transformation, midiDeviceService);
				}
				//else create new transformations
			}

			//delete transformations that dont exist in inProfile
		}
	}
}
