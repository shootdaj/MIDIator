using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Anshul.Utilities;
using Refigure;
using MIDIator.Engine;
using MIDIator.UIGeneration;
using MIDIator.UIGenerator.Consumables;
using Sanford.Multimedia.Midi;
using TypeLite;
using TypeLite.Net4;

namespace MIDIator.UIGenerator
{
	public class CodeGenerator
	{
		private TsModel TsModel { get; set; }

		public CodeGenerator()
		{
		}

		public void Run()
		{
			GenerateDomainModel();
			GenerateComponents();
		}

		private void GenerateDomainModel()
		{
			//var template = new Ng2DomainModel();
			//template.Session = new Dictionary<string, object>
			//{
			//	{"Assemblies", new List<Assembly> {typeof(Profile).Assembly, typeof(ShortMessage).Assembly}}
			//};
			//template.Initialize();

			//var genCode = template.TransformText();

			var ts = TypeScript.Definitions()
				.ForAssembly(typeof(Profile).Assembly)
				.ForAssembly(typeof(ShortMessage).Assembly)
				.WithVisibility((tsClass, name) => true)
				.WithMemberFormatter(identifier => identifier.Name.ToCamelCase());

			TsModel model;
			var genCode = ts.GenerateOutModel(out model).Trim();
			TsModel = model;

			var filename = Config.Get("OutputDirectory").MergePath(Config.Get("DomainModelFileName"));
			Directory.CreateDirectory(Path.GetDirectoryName(filename));
			File.WriteAllText(filename, genCode);
		}

		private void GenerateComponents()
		{
			var domainModelDirective = new ImportDirective("./" + Config.Get("DomainModelFileName").Replace(".ts", string.Empty));
			TsModel.Classes.ToList().ForEach(@class => domainModelDirective.Classes.Add(@class.Name));

			var importDirectives =
				UIGenerationSettings.GlobalImportDirectives.Union(domainModelDirective.Listify()).ToList();

			TsModel.Classes.Where(tsClass => tsClass.Type.GetCustomAttributes(typeof(Ng2ComponentAttribute)).Any())
				.ToList()
				.ForEach(tsClass =>
				{
					var template = new Ng2Component();
					template.Session = new Dictionary<string, object>
					{
						{"BaseType", tsClass},
						{"ImportDirectives", importDirectives}
					};
					template.Initialize();

					var genCode = template.TransformText().Trim();

					var filename = Config.Get("OutputDirectory").MergePath("Components").MergePath($"{tsClass.Name.ToPascalCase()}.component.ts");
					Directory.CreateDirectory(Path.GetDirectoryName(filename));
					File.WriteAllText(filename, genCode);
				});
		}
	}
}
