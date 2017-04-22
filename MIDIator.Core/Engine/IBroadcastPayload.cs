using MIDIator.Interfaces;
using Sanford.Multimedia.Midi;

namespace MIDIator.Engine
{
    public interface IBroadcastPayload
    {
        ChannelMessage IncomingMessage { get; set; }
        ChannelMessage OutgoingMessage { get; set; }
        IMIDIInputDevice InputDevice { get; set; }
        IMIDIOutputDevice OutputDevice { get; set; }
        ITranslation Translation { get; set; }
    }
}