using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http.Formatting;
using System.Web.Http;
using Anshul.Utilities;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.Owin.Cors;
using MIDIator.Engine;
using MIDIator.Json;
using Newtonsoft.Json;
using Owin;
using Refigure;
using Sanford.Multimedia.Midi;

namespace MIDIator.Web
{
	public class Startup
	{
		public void Configuration(IAppBuilder app)
		{
			HttpConfiguration config = new HttpConfiguration();
			config.MapHttpAttributeRoutes();
			config.Routes.MapHttpRoute(
				name: "DefaultApi",
				routeTemplate: "{controller}/{action}",
				defaults: new { controller = "MIDIManager", action = "Index" }

			);

			var signalRConfig = new HubConfiguration { Resolver = new DefaultDependencyResolver() };

			//ignore glimpse route
			config.Routes.IgnoreRoute("Glimpse", "{resource}.axd/{*pathInfo}");

			//always return json
			config.Formatters.Clear();
			config.Formatters.Add(new JsonMediaTypeFormatter());

			//set default serialization settings
			config.Formatters.JsonFormatter.SerializerSettings = SerializerSettings.DefaultSettings;
			JsonConvert.DefaultSettings = () => SerializerSettings.DefaultSettings;

			//add api services
			app.UseCors(CorsOptions.AllowAll);
			app.MapSignalR(signalRConfig);
			app.UseWebApi(config);
        }
    }
}
