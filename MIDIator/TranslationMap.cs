using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace MIDIator
{
    [DataContract]
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

		[DataMember]
		public List<ITranslation> Translations { get; set; }
	}
}
