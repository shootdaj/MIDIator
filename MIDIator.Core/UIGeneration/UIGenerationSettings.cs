using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Anshul.Utilities;

namespace MIDIator.UIGeneration
{
	public class ImportDirective
	{
		public ImportDirective(string module, IList<string> classes = null)
		{
			Classes = classes ?? new List<string>();
			Module = module;
		}

		public IList<string> Classes { get; private set; }
		public string Module { get; private set; }
	}

	public class UIGenerationSettings
	{
		public static IList<ImportDirective> GlobalImportDirectives { get; set; } = new List<ImportDirective>
			{
				new ImportDirective("@angular/core", new List<string>() {"Component", "ViewChild", "Injectable"}),
				new ImportDirective("@angular/forms", new List<string>() {"FormsModule", "ReactiveFormsModule", "FormGroup", "FormControl", "Validators", "FormBuilder"}),
				new ImportDirective("@angular/http", new List<string>() {"Http"}),
				new ImportDirective("rxjs/Observable", new List<string>() {"Observable"}),
				new ImportDirective("./rxjs-operators"),
				new ImportDirective("enum-values", new List<string>() {"EnumValues"}),
				//new ImportDirective("./base", new List<string>() {"MIDIInputDevice", "MIDIOutputDevice", "Translation", "ShortMessage", "InputMatchFunction", "TranslationFunction", "MessageType", "Profile", "Transformation", "ChannelCommand"}),
				new ImportDirective("./midiService", new List<string>() {"MIDIService"}),
				new ImportDirective("./profileService", new List<string>() {"ProfileService"}),
				new ImportDirective("./profile.component", new List<string>() {"ProfileComponent"}),
				new ImportDirective("./mdl-dropdown.component", new List<string>() {"IDropdownOption", "DropdownComponent"})
			};

		//public UIGenerationSettings()
		//{
		//	GlobalImportDirectives 
		//}
	}
}
