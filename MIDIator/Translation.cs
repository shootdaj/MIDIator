using System.ComponentModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Sanford.Multimedia.Midi;

namespace MIDIator
{
    [DataContract]
	[DisplayName(nameof(Translation))]
	public class Translation : ITranslation
	{
	    public Translation()
	    {
	    }

	    public Translation(ShortMessage inputMessageMatchTarget, ShortMessage outputMessageTemplate, InputMatchFunction inputMatchFunction, TranslationFunction translationFunction)
		{
			InputMessageMatchTarget = inputMessageMatchTarget;
			OutputMessageTemplate = outputMessageTemplate;
			InputMatchFunction = inputMatchFunction;
			TranslationFunction = translationFunction;
		}

        public Translation(ShortMessage inputMessageMatchTarget, ShortMessage outputMessageTemplate)
        {
            InputMessageMatchTarget = inputMessageMatchTarget;
            OutputMessageTemplate = outputMessageTemplate;
            InputMatchFunction = InputMatchFunction.Data1Match; //TODO: Replace this with InputMatchFunctions.GetReasonableFunction();
            TranslationFunction = TranslationFunction.DirectTranslation; //TODO: Replace this with TranslationFunctions.GetReasonableFunction();
        }

        [DataMember]
		[JsonProperty(TypeNameHandling = TypeNameHandling.All)]
        public ShortMessage InputMessageMatchTarget { get; set; }

        [DataMember]
		[JsonProperty(TypeNameHandling = TypeNameHandling.All)]
		public ShortMessage OutputMessageTemplate { get; set; }

		[DataMember]
		[JsonConverter(typeof(StringEnumConverter))]
		public TranslationFunction TranslationFunction { get; set; }

		[DataMember]
		[JsonConverter(typeof(StringEnumConverter))]
		public InputMatchFunction InputMatchFunction { get; set; }
	}
}
