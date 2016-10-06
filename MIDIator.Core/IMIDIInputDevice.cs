using System;
using Sanford.Multimedia;
using Sanford.Multimedia.Midi;

namespace MIDIator
{
	public interface IMIDIInputDevice
	{
		int DeviceID { get; }
		ITranslationMap TranslationMap { get; set; }
		bool IsRecording { get; }
		string Name { get; }
		int DriverVersion { get; }
		short MID { get; }
		short PID { get; }
		int Support { get; }
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
	}
}