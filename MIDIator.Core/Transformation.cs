using System;
using TypeLite;

namespace MIDIator
{
	[TsClass]
	public class Transformation : IDisposable
	{
		public string Name { get; set; }
		private string ForwardActionName { get; } = "Forward";
		public IMIDIInputDevice InputDevice { get; set; }
		public IMIDIOutputDevice OutputDevice { get; set; }

		public ITranslationMap TranslationMap => InputDevice.TranslationMap;

		public Transformation(string name, IMIDIInputDevice inputDevice, IMIDIOutputDevice outputDevice, ITranslationMap translationMap)
		{
			Name = name;
			InputDevice = inputDevice;
			OutputDevice = outputDevice;
			InputDevice.TranslationMap = translationMap;
			InputDevice.AddChannelMessageAction(new ChannelMessageAction(message => true, OutputDevice.Send, ForwardActionName));
		}

		public void Dispose()
		{
			InputDevice.RemoveChannelMessageAction(ForwardActionName);
		}
	}
}
