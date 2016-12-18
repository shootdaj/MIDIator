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
			var ts = TypeScript.Definitions()
				.ForAssembly(typeof(Profile).Assembly)
				.ForAssembly(typeof(ShortMessage).Assembly)
				.WithVisibility((tsClass, name) => true)
				.WithMemberFormatter(identifier => identifier.Name.ToCamelCase())
				.AsConstEnums(false);

			TsModel model;
			var genCode = ts.GenerateOutModel(out model).Trim();
			TsModel = model;

			var filename = Config.Get("OutputDirectory").MergePath("models/domainModel.ts");
			Directory.CreateDirectory(Path.GetDirectoryName(filename));
			File.WriteAllText(filename, genCode);
		}

		private void GenerateComponents()
		{
			var domainModelDirective = new ImportDirective("../../models/domainModel");
			TsModel.Classes.ToList().ForEach(@class => domainModelDirective.Classes.Add(@class.Name));
			TsModel.Enums.ToList().ForEach(@enum => domainModelDirective.Classes.Add(@enum.Name));

			var importDirectives =
				UIGenerationSettings.GlobalImportDirectives.Union(domainModelDirective.Listify()).ToList();

			//add domain components to directive list
			TsModel.Classes.Where(tsClass => tsClass.Type.GetCustomAttributes(typeof(Ng2ComponentAttribute)).Any())
				.ToList()
				.ForEach(tsClass =>
				{
					importDirectives.Add(new ImportDirective($"../../components/{tsClass.Name.ToCamelCase()}/{tsClass.Name.ToCamelCase()}.component",
						(tsClass.Name + "Component").Listify()));
				});

			//generate component code
			TsModel.Classes.Where(tsClass => tsClass.Type.GetCustomAttributes(typeof(Ng2ComponentAttribute)).Any())
				.ToList()
				.ForEach(tsClass =>
				{
					var template = new Ng2DomainComponent();
					var attribute = (Ng2ComponentAttribute)tsClass.Type.GetCustomAttributes(typeof(Ng2ComponentAttribute)).First();
					template.Session = new Dictionary<string, object>
					{
						{"BaseType", tsClass},
						{
							"ImportDirectives",
							importDirectives.Except(importDirectives.Where(x => x.Classes.Contains(tsClass.Name + "Component"))).ToList()
						},
						{
							"ComponentCodeTemplate",
							attribute.ComponentCodeTemplate
						}
					};
					template.Initialize();

					var genCode = template.TransformText().Trim();

					var filename = Config.Get("OutputDirectory").MergePath($"components/{tsClass.Name.ToCamelCase()}").MergePath($"{tsClass.Name.ToCamelCase()}.component.ts");
					Directory.CreateDirectory(Path.GetDirectoryName(filename));
					File.WriteAllText(filename, genCode);
				});
		}
	}
}
