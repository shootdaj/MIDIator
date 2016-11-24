using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.Serialization;
using MIDIator.Interfaces;
using Newtonsoft.Json;

namespace MIDIator.Engine
{
	public class TranslationMap : ITranslationMap
	{
		public TranslationMap(List<ITranslation> translations = null)
		{
			Translations = translations ?? new List<ITranslation>();
		}

		[JsonConstructor]
		public TranslationMap(List<Translation> translations = null)
		{
			Translations = translations?.Cast<ITranslation>().ToList() ?? new List<Translation>().Cast<ITranslation>().ToList();
		}

		public List<ITranslation> Translations { get; set; }
		public void Update(dynamic translationMap)
		{
			//Translations = translationMap.Translations;
		}
	}
}
