using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace MIDIator.Web.Hubs
{
	public class MIDIHub : Hub
	{
		public IEnumerable<dynamic> AvailableInputDevices()
		{
			return MIDIManager.AvailableInputDevices;
		}
	}
}
