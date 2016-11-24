using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIDIator.DTO
{
	class ProfileDTO
	{
		public string Name { get; set; }

		public IEnumerable<TransformationDTO> Transformations { get; set; }
		
		//public List<VirtualOutputDeviceDTO> VirtualOutputDevices { get; set; }
	}
}
