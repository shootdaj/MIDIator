using System;
using Microsoft.AspNet.SignalR;
using MIDIator.Engine;
using MIDIator.Web.Hubs;

namespace MIDIator.Web
{
    public class SignalRService
    {
	    #region Singleton

	    public static void Instantiate()
	    {
		    Instance = new SignalRService();
	    }

	    public static SignalRService Instance { get; private set; }

	    #endregion

		public IHubContext HubContext { get; }

        public SignalRService()
        {
            HubContext = GlobalHost.ConnectionManager.GetHubContext<MIDIReaderHub>();
        }

        public Action<IBroadcastPayload> SetBroadcastAction => payload =>
        {
            HubContext.Clients.Group(Constants.TaskChannel)
                .OnEvent(Constants.TaskChannel, new ChannelEvent
                {
                    ChannelName = Constants.TaskChannel,
                    Name = "broadcastEvent",
                    Data = payload
                });
        };
    }
}