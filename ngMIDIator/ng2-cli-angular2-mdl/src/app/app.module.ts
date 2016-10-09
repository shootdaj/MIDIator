import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { MdlModule } from 'angular2-mdl';
import { AppComponent } from './app.component';
import { DropdownComponent } from './mdl-dropdown.component';
import { MIDIInputDevice } from './base';
import { MIDIOutputDevice } from './base';
import { Transformation } from './base';
import { Translation } from './base';
import { ShortMessage } from './base';
import { InputMatchFunction } from './base';
import { TranslationFunction } from './base';
import { MessageType } from './base';

import { ProfileComponent } from './profile.component'

@NgModule({
    declarations: [
        AppComponent,
        DropdownComponent,
		ProfileComponent
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
