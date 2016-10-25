using System;

namespace MIDIator.UIGenerator.Consumables
{
	public class Ng2ComponentAttribute : Attribute
	{
		public Ng2ComponentAttribute(Type viewTemplate = null)
		{
			ViewTemplate = viewTemplate;
		}

		public Type ViewTemplate { get; private set; }
	}
}
