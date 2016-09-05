using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using MIDIator.JsonConverters;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Sanford.Multimedia.Midi;

namespace MIDIator
{
    [DataContract]
    //[JsonConverter(typeof(TranslationConverter))]
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
        public ShortMessage InputMessageMatchTarget { get; set; }

        [DataMember]
        public ShortMessage OutputMessageTemplate { get; set; }

		[DataMember]
		[JsonConverter(typeof(StringEnumConverter))]
		public TranslationFunction TranslationFunction { get; set; }

		[DataMember]
		[JsonConverter(typeof(StringEnumConverter))]
		public InputMatchFunction InputMatchFunction { get; set; }

		//[DataMember]
		//[JsonConverter(typeof(BinaryConverter))]
  //      public string dsdfg { get; set; }
	}
}
