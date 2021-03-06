﻿using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using JetBrains.Annotations;
using MIDIator.Interfaces;
using MIDIator.UIGenerator.Consumables;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Sanford.Multimedia.Midi;
using TypeLite;

namespace MIDIator.Engine
{
	[TsClass(Module = "")]
	[DataContract]
	[DisplayName(nameof(Translation))]
	[Ng2Component()]
	public class Translation : ITranslation
	{
        [UsedImplicitly]
	    public Translation()
	    {
	        ID = Guid.NewGuid();
        }

        public Translation(ShortMessage inputMessageMatchTarget, ShortMessage outputMessageTemplate, InputMatchFunction inputMatchFunction, TranslationFunction translationFunction, string name = "", string description = "", Guid? id = null)
		{
			InputMessageMatchTarget = inputMessageMatchTarget;
			OutputMessageTemplate = outputMessageTemplate;
			InputMatchFunction = inputMatchFunction;
			TranslationFunction = translationFunction;
		    Name = name;
		    Description = description;
		    ID = id ?? Guid.NewGuid();
		}

        public Translation(ShortMessage inputMessageMatchTarget, ShortMessage outputMessageTemplate)
        {
            InputMessageMatchTarget = inputMessageMatchTarget;
            OutputMessageTemplate = outputMessageTemplate;
            InputMatchFunction = InputMatchFunction.Data1Match; //TODO: Replace this with InputMatchFunctions.GetReasonableFunction();
            TranslationFunction = TranslationFunction.DirectTranslation; //TODO: Replace this with TranslationFunctions.GetReasonableFunction();
            ID = Guid.NewGuid();
        }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public Guid? ID { get; set; }

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

		[DataMember]
		public bool Enabled { get; set; } = true;

        [DataMember]
	    public bool Collapsed { get; set; } = false;
	}
}
