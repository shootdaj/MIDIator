using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MIDIator.Interfaces;
using Sanford.Multimedia.Midi;

namespace MIDIator.Engine
{
    public class BroadcastPayload
    {
        public ChannelMessage IncomingMessage { get; set; }
        public ChannelMessage OutgoingMessage { get; set; }
        public IMIDIInputDevice InputDevice { get; set; }
        public IMIDIOutputDevice OutputDevice { get; set; }
        public ITranslation Translation { get; set; }

        public BroadcastPayload()
        {
            
        }
    }
}
