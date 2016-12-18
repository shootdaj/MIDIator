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
				new ImportDirective("@angular/core", new[] {"Component", "ViewChild", "Injectable", "Input", "Output", "EventEmitter", "DoCheck"}),
				new ImportDirective("@angular/forms", new[] { "FormsModule", "ReactiveFormsModule", "FormGroup", "FormControl", "Validators", "FormBuilder"}),
				new ImportDirective("@angular/http", "Http".Listify()),
				new ImportDirective("rxjs/Observable", "Observable".Listify()),
				new ImportDirective("rxjs/Subscription", "Subscription".Listify()),
                new ImportDirective("rxjs/Subject", "Subject".Listify()),
                new ImportDirective("./rxjs-operators"),
				new ImportDirective("enum-values", "EnumValues".Listify()),
				new ImportDirective("../../services/midiService", "MIDIService".Listify()),
				new ImportDirective("../../services/profileService", "ProfileService".Listify()),
				new ImportDirective("../../components/mdl-dropdown/mdl-dropdown.component", new[] {"DropdownOption", "DropdownComponent"}),
			};
	}
}
