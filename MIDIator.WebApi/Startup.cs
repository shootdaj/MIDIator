using System.Web.Http;
using Microsoft.Owin.Cors;
using MIDIator.Json;
using Owin;

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

			config.Routes.IgnoreRoute("Glimpse", "{resource}.axd/{*pathInfo}");
			config.Formatters.JsonFormatter.SerializerSettings = SerializerSettings.DefaultSettings;

			app.UseCors(CorsOptions.AllowAll);
			app.UseWebApi(config);
		}
	}
}
