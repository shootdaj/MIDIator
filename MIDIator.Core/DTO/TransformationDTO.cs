using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MIDIator.Interfaces;

namespace MIDIator.DTO
{
	class TransformationDTO
	{
		public int DeviceID { get; set; }
		public TranslationMapDTO TranslationMap { get; set; }
		public string Name { get; set; }
	}
}
