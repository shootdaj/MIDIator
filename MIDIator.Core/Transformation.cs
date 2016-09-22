using System;

namespace MIDIator
{
	public class Transformation : IDisposable
	{
		public string Name { get; set; }
		private string ForwardActionName { get; } = "Forward";
		public MIDIInputDevice InputDevice { get; set; }
		public MIDIOutputDevice OutputDevice { get; set; }

		public ITranslationMap TranslationMap => InputDevice.TranslationMap;

		public Transformation(string name, IMIDIInputDevice inputDevice, IMIDIOutputDevice outputDevice, ITranslationMap translationMap)
		{
			Name = name;
			inputDevice.TranslationMap = translationMap;
			inputDevice.AddChannelMessageAction(new ChannelMessageAction(message => true, outputDevice.Send, ForwardActionName));
		}

		public void Dispose()
		{
			InputDevice.RemoveChannelMessageAction(ForwardActionName);
		}
	}
}
