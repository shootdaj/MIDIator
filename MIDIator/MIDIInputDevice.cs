using System;
using System.Collections.Generic;
using System.Linq;
using Sanford.Multimedia;
using Sanford.Multimedia.Midi;

namespace MIDIator
{
	public class MIDIInputDevice : InputDevice
	{
		public MIDIInputDevice(int deviceID, ITranslationMap translationMap = null) : base(deviceID)
		{
			TranslationMap = translationMap;
			ChannelMessageReceived += MIDIInputDevice_ChannelMessageReceived;
		}

		private void MIDIInputDevice_ChannelMessageReceived(object sender, ChannelMessageEventArgs e)
		{
			ExecuteChannelMessageAction(e);
		}

		private void ExecuteChannelMessageAction(ChannelMessageEventArgs channelMessageEventArgs)
		{
			var incomingMessage = channelMessageEventArgs.Message;
			var translations = TranslationMap?.Translations.Where(t => t.InputMatchFunction(incomingMessage)).ToList();
			if (translations != null && translations.Any())
			{
				TranslationMap?.Translations.Where(t => t.InputMatchFunction(incomingMessage))
					.ToList()
					.ForEach(translation =>
					{
						ChannelMessageActions.Where(channelMessageAction => channelMessageAction.Match.MatchFunction(incomingMessage))
							.ToList()
							.ForEach(c =>
							{
								var translatedMessage = translation.TranslationFunction(incomingMessage, translation.OutputMessageTemplate);
								c.Action(translatedMessage.ToChannelMessage());
							});
					});
			}
			else
			{
				ChannelMessageActions.Where(channelMessageAction => channelMessageAction.Match.MatchFunction(incomingMessage))
					.ToList()
					.ForEach(c =>
					{
						c.Action(incomingMessage.ToChannelMessage());
					});
			}
		}

		private List<ChannelMessageAction> ChannelMessageActions { get; } = new List<ChannelMessageAction>();

		//   /// <summary>
		///// Sets up the Translation Map events
		///// </summary>
		//private void SetupTranslationMap()
		//   {
		//    TranslationMap.Translations.ForEach(t => AddChannelMessageAction((sender, args) =>
		//    {
		//	    if (t.InputMatchFunction(args.Message))
		//	    {

		//	    }
		//    }));
		//   }

		private void GetMessageType(ShortMessage shortMessage)
		{

		}

		public ITranslationMap TranslationMap { get; private set; }

		public bool IsRecording { get; protected set; }

		public string Name => GetDeviceCapabilities(DeviceID).name;

		public int DriverVersion => GetDeviceCapabilities(DeviceID).driverVersion;

		public short MID => GetDeviceCapabilities(DeviceID).mid;

		public short PID => GetDeviceCapabilities(DeviceID).pid;

		public int Support => GetDeviceCapabilities(DeviceID).support;

		public void Start()
		{
			StartRecording();
			IsRecording = true;
		}

		public void Stop()
		{
			StopRecording();
			IsRecording = false;
		}

		public void AddChannelMessageAction(ChannelMessageAction channelMessageAction)
		{
			ChannelMessageActions.Add(channelMessageAction);
		}

		public void AddSysCommonMessageAction(EventHandler<SysCommonMessageEventArgs> action)
		{
			var wasRecording = IsRecording;
			if (IsRecording)
				Stop();
			SysCommonMessageReceived += action;

			if (wasRecording)
				Start();
		}

		public void AddSysExMessageAction(EventHandler<SysExMessageEventArgs> action)
		{
			var wasRecording = IsRecording;
			if (IsRecording)
				Stop();
			SysExMessageReceived += action;

			if (wasRecording)
				Start();
		}

		public void AddSysRealtimeMessageAction(EventHandler<SysRealtimeMessageEventArgs> action)
		{
			var wasRecording = IsRecording;
			if (IsRecording)
				Stop();
			SysRealtimeMessageReceived += action;

			if (wasRecording)
				Start();
		}

		public void AddErrorAction(EventHandler<ErrorEventArgs> action)
		{
			var wasRecording = IsRecording;
			if (IsRecording)
				Stop();
			Error += action;

			if (wasRecording)
				Start();
		}

		public new void Dispose()
		{
			Reset();
			base.Dispose();
		}
	}
}