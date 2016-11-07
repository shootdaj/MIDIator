import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { MdlModule } from 'angular2-mdl';

//domain model
import { IMIDIInputDevice, ITranslationMap, ITranslation, ShortMessage, IMIDIOutputDevice, Transformation, Profile, VirtualOutputDevice, VirtualDevice, MIDIOutputDevice, MIDIInputDevice, Translation, ChannelMessage, MessageType, TranslationFunction, InputMatchFunction, ChannelCommand, IDropdownOption } from './models/domainModel';

//services
import { MIDIService } from './services/midiService';

//components
import { AppComponent } from './components/app/app.component';
import { DropdownComponent } from './components/mdl-dropdown/mdl-dropdown.component';

@NgModule({
    declarations: [
        AppComponent,
        DropdownComponent
	],
    imports: [
        BrowserModule,
		ReactiveFormsModule,
		FormsModule,
        HttpModule,
        MdlModule
    ],
    providers: [],
    bootstrap: [AppComponent]
})
export class AppModule { }
