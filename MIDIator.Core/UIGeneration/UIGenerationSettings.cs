using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Anshul.Utilities;
using MIDIator.UIGenerator.Consumables;

namespace MIDIator.UIGeneration
{
	public class UIGenerationSettings
	{
		public static IList<ImportDirective> GlobalImportDirectives { get; set; } = new List<ImportDirective>
			{
				new ImportDirective("@angular/core", new List<string>() {"Component", "ViewChild", "Injectable", "Input", "Output", "EventEmitter"}),
				new ImportDirective("@angular/forms", new List<string>() {"FormsModule", "ReactiveFormsModule", "FormGroup", "FormControl", "Validators", "FormBuilder"}),
				new ImportDirective("@angular/http", new List<string>() {"Http"}),
				new ImportDirective("rxjs/Observable", new List<string>() {"Observable"}),
				new ImportDirective("rxjs/Subscription", new List<string>() {"Subscription"}),
				new ImportDirective("./rxjs-operators"),
				new ImportDirective("enum-values", new List<string>() {"EnumValues"}),
				new ImportDirective("./midiService", new List<string>() {"MIDIService"}),
				new ImportDirective("./profileService", new List<string>() {"ProfileService"}),
				new ImportDirective("./mdl-dropdown.component", new List<string>() {"IDropdownOption", "DropdownComponent"}),
			};
	}
}
