using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MIDIator
{
    [DataContract]
	public class TranslationMap : ITranslationMap
	{
		public TranslationMap(List<ITranslation> translations = null)
		{
			Translations = translations ?? new List<ITranslation>();
		}

		[DataMember]
		public List<ITranslation> Translations { get; set; }
	}
}
