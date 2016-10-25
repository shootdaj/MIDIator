using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MIDIator.Engine;
using MIDIator.UIGenerator.Consumables;
using Refigure;
using Sanford.Multimedia.Midi;
using TypeLite;
using TypeLite.Net4;

namespace MIDIator.UIGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
			//var ts = TypeScript.Definitions()
			//	//.For(typeof(MIDIInputDevice))
			//	.ForAssembly(typeof(Profile).Assembly)
			//	.ForAssembly(typeof(ShortMessage).Assembly)
			//	.WithVisibility((tsClass, name) => true)
			//	.WithMemberFormatter(identifier => identifier.Name.ToCamelCase());

			//      TsModel model;
			//      var genCode = ts.GenerateOutModel(out model);



			//      var filename = Config.Get("OutputDirectory").MergePath(Config.Get("DomainModelFileName"));
			//Directory.CreateDirectory(Path.GetDirectoryName(filename));
			//File.WriteAllText(filename, genCode);

			var codeGen = new CodeGenerator();
			codeGen.Run();

			//Console.ReadLine();
        }
    }
}
