using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Refigure;

namespace MIDIator.UIGenerator
{
	public class CodeGenerator
	{
		public CodeGenerator(params Type[] baseType)
		{
			BaseType = baseType;
		}

		public IEnumerable<Type> BaseType { get; }
		
		public void Run()
		{
			GenerateBaseTS();
		}

		private void GenerateBaseTS()
		{
			var template = new Ng2Base();
			//template.Session = new Dictionary<string, object> {{"BaseType", BaseType}};
			template.Initialize();

			var genCode = template.TransformText();
			
			var filename = Config.Get("OutputDirectory").MergePath("base.ts");
			Directory.CreateDirectory(Path.GetDirectoryName(filename));

			File.WriteAllText(filename, genCode);
		}
	}
}
