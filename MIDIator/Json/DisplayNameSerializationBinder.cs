using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Newtonsoft.Json.Serialization;
using Sanford.Multimedia.Midi;

namespace MIDIator.Json
{
	public class DisplayNameSerializationBinder : DefaultSerializationBinder
	{
		private Dictionary<string, Type> _nameToType;
		private Dictionary<Type, string> _typeToName;

		public DisplayNameSerializationBinder()
		{
			var customDisplayNameTypes =
				this.GetType()
					.Assembly
					.GetTypes().Union(typeof(ShortMessage).Assembly.GetTypes()) //also check Sanford lib
					.Where(x => x
						.GetCustomAttributes(false)
						.Any(y => y is DisplayNameAttribute));

			_nameToType = customDisplayNameTypes.ToDictionary(
				t => t.GetCustomAttributes(false).OfType<DisplayNameAttribute>().First().DisplayName,
				t => t);

			_typeToName = _nameToType.ToDictionary(
				t => t.Value,
				t => t.Key);

		}

		public override void BindToName(Type serializedType, out string assemblyName, out string typeName)
		{
			if (false == _typeToName.ContainsKey(serializedType))
			{
				base.BindToName(serializedType, out assemblyName, out typeName);
				return;
			}

			var name = _typeToName[serializedType];

			assemblyName = null;
			typeName = name;
		}

		public override Type BindToType(string assemblyName, string typeName)
		{
			if (_nameToType.ContainsKey(typeName))
				return _nameToType[typeName];

			return base.BindToType(assemblyName, typeName);
		}
	}
}
