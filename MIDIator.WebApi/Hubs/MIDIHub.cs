using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using MIDIator.Engine;

namespace MIDIator.Web.Hubs
{
	public class MIDIHub : Hub
	{
		private MIDIManager MIDIManager { get; set; }
		
		public IEnumerable<dynamic> AvailableInputDevices()
		{
			return MIDIManager.AvailableInputDevices;
		}
	}
}
