using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MIDIator.Engine;
using MIDIator.Interfaces;

namespace MIDIator.DTO
{
	class MIDIInputDeviceDTO
	{
		public int DeviceID { get; set; }
		public TranslationMap TranslationMap { get; set; }
		public string Name { get; set; }
	}
}
