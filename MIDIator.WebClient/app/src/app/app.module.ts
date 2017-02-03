import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { MdlModule } from 'angular2-mdl';
import { SlimLoadingBarModule } from 'ng2-slim-loading-bar';
import { CustomFormsModule } from 'ng2-validation'

//domain model
import { IMIDIInputDevice, TranslationMap, ShortMessage, IMIDIOutputDevice, Transformation, Profile, VirtualOutputDevice, VirtualDevice, MIDIOutputDevice, MIDIInputDevice, Translation, ChannelMessage, MessageType, TranslationFunction, InputMatchFunction, ChannelCommand, IDropdownOption } from './models/domainModel';

//services
import { MIDIService } from './services/midiService';
import { FormService } from './services/formService';
import { RealtimeService } from './services/realtimeService';
import { ProfileService } from './services/profileService';
import { HelperService } from './services/helperService';
import { SignalRService, SignalrWindow, ChannelConfig } from './services/signalRService';

//components
import { AppComponent } from './components/app/app.component';
import { DropdownComponent } from './components/mdl-dropdown/mdl-dropdown.component';
import { ProfileComponent } from './components/profile/profile.component';
import { TransformationComponent } from './components/transformation/transformation.component';
import { TranslationComponent } from './components/translation/translation.component';
import { ChannelMessageComponent } from './components/channelMessage/channelMessage.component';
import { TextInputComponent } from './components/mdl-textinput/mdl-textinput.component';
import { ExpanderComponent } from './components/expander/expander.component';

export function channelConfigFactory() {
    let channelConfig = new ChannelConfig();
    channelConfig.url = "http://localhost:9000/signalr";
    channelConfig.hubName = "MIDIReaderHub";
    return channelConfig;
}



@NgModule({
    declarations: [
        AppComponent,
		ProfileComponent,
		TransformationComponent,
        DropdownComponent,
		TranslationComponent,
		ChannelMessageComponent,
        TextInputComponent,
        ExpanderComponent
	],
    imports: [
        BrowserModule,
		ReactiveFormsModule,
		FormsModule,
        HttpModule,
        MdlModule,
        SlimLoadingBarModule.forRoot(),
		CustomFormsModule
    ],
    providers: [MIDIService, HelperService, ProfileService, FormService, RealtimeService, SignalRService,
        { provide: SignalrWindow, useValue: window },
        { provide: 'channel.config', useValue: channelConfigFactory }],
    bootstrap: [AppComponent]
})
export class AppModule { }
