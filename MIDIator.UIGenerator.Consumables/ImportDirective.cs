using System.Collections.Generic;

namespace MIDIator.UIGenerator.Consumables
{
	public class ImportDirective
	{
		public ImportDirective(string module, IList<string> classes = null)
		{
			Classes = classes ?? new List<string>();
			Module = module;
		}

		public IList<string> Classes { get; private set; }
		public string Module { get; private set; }
	}
}