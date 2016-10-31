//domain model
import { IMIDIInputDevice, ITranslationMap, ITranslation, ShortMessage, IMIDIOutputDevice, Transformation, Profile, VirtualOutputDevice, VirtualDevice, MIDIOutputDevice, MIDIInputDevice, Translation, ChannelMessage, MessageType, TranslationFunction, InputMatchFunction, ChannelCommand } from '../../models/domainModel';

//services
import { MIDIService } from '../../services/midiService';
import { ProfileService } from '../../services/profileService';

//components
import { IDropdownOption, DropdownComponent } from '../../components/mdl-dropdown/mdl-dropdown.component';
import { ProfileComponent } from '../../components/profile/profile.component';

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
	selector: 'transformation',
    templateUrl: './transformation.component.html',
    providers: [MIDIService]
})

export class TransformationComponent {
    private subscriptions: Subscription[];
    private currentTransformation: Transformation;
    private availableInputDevices: MIDIInputDevice[];
    private availableOutputDevices: MIDIOutputDevice[];
	
	@Input() set transformation(inTransformation: Transformation){
		this.currentTransformation = inTransformation;
		this.transformationChange.emit(inTransformation); 
	}
	get transformation() : Transformation {
		return this.currentTransformation; 
	}
	
    @Output() transformationChange: EventEmitter<Transformation> = new EventEmitter<Transformation>();

    constructor(private midiService: MIDIService) {
        this.subscriptions.push(this.midiService.availableInputDevicesSubject
            .subscribe(data => this.availableInputDevices = data));

        this.subscriptions.push(this.midiService.availableOutputDevicesSubject
            .subscribe(data => this.availableOutputDevices = data));
    }
}