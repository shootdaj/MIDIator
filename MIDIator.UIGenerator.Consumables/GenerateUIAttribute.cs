using System;

namespace MIDIator.UIGenerator.Consumables
{
	public class GenerateUIAttribute : Attribute
	{
		public GenerateUIAttribute(Type viewTemplate = null)
		{
			ViewTemplate = viewTemplate;
		}

		public Type ViewTemplate { get; private set; }
	}
}
