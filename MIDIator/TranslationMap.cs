using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MIDIator
{
	[Serializable]
	public class TranslationMap : ITranslationMap
	{
		public TranslationMap(List<ITranslation> translations = null)
		{
			Translations = translations ?? new List<ITranslation>();
		}

		public List<ITranslation> Translations { get; set; }
	}
}
