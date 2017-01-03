using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Anshul.Utilities;
using MIDIator.UIGeneration;
using MIDIator.UIGenerator.Consumables;
using Newtonsoft.Json;
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

        [JsonIgnore]
        public BetterList<VirtualLoopbackDevice> VirtualLoopbackDevices { get; set; }

        [JsonProperty(nameof(VirtualLoopbackDevices))]
        private BetterList<VirtualLoopbackDevice> VirtualLoopbackDevicesJson => VirtualLoopbackDevices;

	    public void Update(ExpandoObject inProfile, MIDIDeviceService midiDeviceService, VirtualMIDIManager virtualMIDIManager)
		{
			dynamic profile = inProfile;

			Name = profile.Name;

            var transformationNames = new List<string>();

			foreach (var inTransformation in profile.Transformations)
			{
                transformationNames.Add(inTransformation.Name);

				var matchedTransformations = Transformations.Where(t => t.Name == inTransformation.Name);

				//TODO: check which transformations are different - currently updating all transformations
                var matchedTransformationsList = matchedTransformations.ToList();
				if (matchedTransformationsList.Any())
				{
					var matchedTransformation = matchedTransformationsList.Single();
					matchedTransformation.Update(inTransformation, midiDeviceService, virtualMIDIManager);
				}
				else
				{
					//TODO: Test
					//else create new transformations	
					var newTransformation = new Transformation(inTransformation.Name, inTransformation, midiDeviceService,
						virtualMIDIManager);
					Transformations.Add(newTransformation);
				}
			}

            //delete xforms that don't exist in inProfile
		    Transformations.RemoveAll(t => !transformationNames.Contains(t.Name));
		}
	}
}

