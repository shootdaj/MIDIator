using MIDIator.Interfaces;
using Sanford.Multimedia.Midi;

namespace MIDIator.Engine
{
    public class BroadcastPayload : IBroadcastPayload
    {
        public ChannelMessage IncomingMessage { get; set; }
        public ChannelMessage OutgoingMessage { get; set; }
        public IMIDIInputDevice InputDevice { get; set; }
        public IMIDIOutputDevice OutputDevice { get; set; }
        public ITranslation Translation { get; set; }

        public BroadcastPayload()
        {
            
        }

        public static BroadcastPayload GetBroadcastPayload(ChannelMessage incomingMessage,
            ChannelMessage outgoingMessage = null,
            ITranslation translation = null, IMIDIInputDevice inputDevice = null,
            IMIDIOutputDevice outputDevice = null)
        {
            return new BroadcastPayload
            {
                IncomingMessage = incomingMessage,
                OutgoingMessage = outgoingMessage,
                Translation = translation,
                InputDevice = inputDevice,
                OutputDevice = outputDevice,
            };
        }
    }
}
