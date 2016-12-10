using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MIDIator.VirtualMIDI;
using Sanford.Multimedia.Midi;

namespace MIDIator.Engine
{
	public class VirtualLoopbackDevice : VirtualDevice
	{
		public bool Running { get; private set; } = false;

		public VirtualLoopbackDevice(string name, ref Guid manufacturerID, ref Guid productID) 
			: base(name, ref manufacturerID, ref productID, VirtualDeviceType.Loopback)
		{

		}

		public void Start()
		{
			try
			{
				Running = true;
				Task.Run(() =>
				{
					while (Running) //technically this will wait for one final command before quitting
									//but it will be shutdown during the final dispose
					{
						//if (Running)
						//{
						//	try
						//	{
								var command = TeVirtualMIDIDevice.getCommand();
								TeVirtualMIDIDevice.sendCommand(command);
							//}
							//catch (TeVirtualMIDIException exception)
							//{
							//	if (exception.Message == "Port not enabled")
							//	{
							//		Console.WriteLine("final loop access to getCommand()");
							//	}
							//	else
							//	{
							//		throw;
							//	}
							//}
							//catch (NullReferenceException)
							//{
							//	Console.WriteLine("final loop access to getCommand()");

							//	//if (exception.Message == "Port not enabled")
							//	//{
							//	//	Console.WriteLine("final loop access to getCommand()");
							//	//}
							//	//else
							//	//{
							//	//	throw;
							//	//}
							//}
						//}
					}
				});
			}
			catch (Exception ex)
			{
				throw new Exception("Exception during loopback", ex);
			}
		}

		public void Stop()
		{
			Running = false;
		}

		public override void Dispose()
		{
			Stop();
			base.Dispose();
		}

		//public void Stop()
		//{

		//}
	}
}
