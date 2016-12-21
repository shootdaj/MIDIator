using System;
using System.Collections.Generic;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.Hosting;
using Microsoft.Owin.StaticFiles;
using Owin;
using Refigure;

namespace MIDIator.WebClient
{
	public class WebClientManager
	{
		private IDisposable Server { get; set; }

		public void InitializeWebClient(string baseAddress)
		{
			Server = StartStaticFileServer(baseAddress);
		}

		private static IDisposable StartStaticFileServer(string baseAddress)
		{
			var root = "app/dist";
			var fileSystem = new PhysicalFileSystem(root);

			var options = new FileServerOptions
			{
				EnableDirectoryBrowsing = true,
				FileSystem = fileSystem
			};

			return WebApp.Start(baseAddress, builder => builder.UseFileServer(options));
		}

		public void DisposeWebClient()
		{
			Server.Dispose();
			Server = null;
		}
	}
}
