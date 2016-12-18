using System;

namespace MIDIator.UIGenerator.Consumables
{
	public class Ng2ComponentAttribute : Attribute
	{
		public Ng2ComponentAttribute(Type viewTemplate = null, Type componentCodeTemplate = null)//, string filePath = null)//, Type[] dependentTypes = null)
		{
			//FilePath = filePath;
			ViewTemplate = viewTemplate;
		    ComponentCodeTemplate = componentCodeTemplate;
		}

		public Type ViewTemplate { get; private set; }
		public Type ComponentCodeTemplate { get; private set; }
		//public string FilePath { get; private set; }
	}
}
