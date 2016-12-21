using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.Hosting;
using Microsoft.Owin.StaticFiles;
using Owin;
using Refigure;

namespace MIDIator.WebClient
{
	class Program
	{
		private static readonly WebClientManager WebClientManager = new WebClientManager();
		
		static void Main(string[] args)
		{
			var baseAddress = Config.Get("WebClient.BaseAddress");
			WebClientManager.InitializeWebClient(baseAddress);
			Console.WriteLine("Listening at " + baseAddress);
			Console.ReadLine();
			WebClientManager.DisposeWebClient();
		}
	}
}
