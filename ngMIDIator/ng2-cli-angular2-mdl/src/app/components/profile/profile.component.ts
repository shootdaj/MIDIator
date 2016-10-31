//domain model
import { IMIDIInputDevice, ITranslationMap, ITranslation, ShortMessage, IMIDIOutputDevice, Transformation, Profile, VirtualOutputDevice, VirtualDevice, MIDIOutputDevice, MIDIInputDevice, Translation, ChannelMessage, MessageType, TranslationFunction, InputMatchFunction, ChannelCommand } from '../../models/domainModel';

//services
import { MIDIService } from '../../services/midiService';
import { ProfileService } from '../../services/profileService';

//components
import { IDropdownOption, DropdownComponent } from '../../components/mdl-dropdown/mdl-dropdown.component';
import { TransformationComponent } from '../../components/transformation/transformation.component';

//ng2
import { Component, ViewChild, Injectable, Input, Output, EventEmitter, DoCheck } from '@angular/core';
import { FormsModule, ReactiveFormsModule, FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { Http } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { Subscription } from 'rxjs/Subscription';
import { Subject } from 'rxjs/Subject';
import './rxjs-operators';

//libs
import { EnumValues } from 'enum-values';

@Component({
    selector: 'profile',
    templateUrl: './profile.component.html'
})

export class ProfileComponent {
    private currentProfile: Profile;
    private form: FormGroup;

    @Input() set profile(inProfile: Profile) {
        this.currentProfile = inProfile;
        this.profileChange.emit(inProfile);
    }
    get profile(): Profile {
        return this.currentProfile;
    }

    @Output() profileChange: EventEmitter<Profile> = new EventEmitter<Profile>();

    constructor(fb: FormBuilder) {
        this.form = fb.group({
            "name": this.currentProfile.name,
            "transformations": this.currentProfile.transformations
        });
    }
}