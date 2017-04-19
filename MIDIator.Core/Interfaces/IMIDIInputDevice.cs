using System;
using System.Runtime.Serialization;
using MIDIator.Engine;
using Sanford.Multimedia;
using Sanford.Multimedia.Midi;
using TypeLite;

namespace MIDIator.Interfaces
{
	[TsInterface(Module = "")]
	public interface IMIDIInputDevice
	{
		int DeviceID { get; }
		ITranslationMap TranslationMap { get; set; }
		string Name { get; }
		bool IsRecording { get; }
		int DriverVersion { get; }
		short MID { get; }
		short PID { get; }
		int Support { get; }
		int ChannelMessageActionCount { get; }
		void Start();
		void Stop();
		void AddChannelMessageAction(ChannelMessageAction channelMessageAction);
		void AddSysCommonMessageAction(EventHandler<SysCommonMessageEventArgs> action);
		void AddSysExMessageAction(EventHandler<SysExMessageEventArgs> action);
		void AddSysRealtimeMessageAction(EventHandler<SysRealtimeMessageEventArgs> action);
		void AddErrorAction(EventHandler<ErrorEventArgs> action);

		/// <summary>
		/// Removes all channel message actions that have the given name.
		/// </summary>
		/// <param name="name"></param>
		void RemoveChannelMessageAction(string name);

		void StartMIDIReader(Action<ChannelMessageEventArgs> messageAction);
		void StopMIDIReader();
	    void SetBroadcastAction(Action<BroadcastPayload> broadcastAction);
	    void RemoveBroadcastAction();
	}
}