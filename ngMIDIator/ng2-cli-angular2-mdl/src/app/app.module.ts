import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { MdlModule } from 'angular2-mdl';

//domain model
import { IMIDIInputDevice, ITranslationMap, ITranslation, ShortMessage, IMIDIOutputDevice, Transformation, Profile, VirtualOutputDevice, VirtualDevice, MIDIOutputDevice, MIDIInputDevice, Translation, ChannelMessage, MessageType, TranslationFunction, InputMatchFunction, ChannelCommand } from './models/domainModel';

//services
import { MIDIService } from './services/midiService';
import { ProfileService } from './services/profileService';

//components
import { AppComponent } from './components/app/app.component';
import { IDropdownOption, DropdownComponent } from './components/mdl-dropdown/mdl-dropdown.component';
import { TransformationComponent } from './components/transformation/transformation.component';
import { ProfileComponent } from './components/profile/profile.component';

@NgModule({
    declarations: [
        AppComponent,
        DropdownComponent,
		ProfileComponent,
		TransformationComponent
    ],
    imports: [
        BrowserModule,
        FormsModule,
        HttpModule,
        MdlModule
    ],
    providers: [],
    bootstrap: [AppComponent]
})
export class AppModule { }
