import { Component, ViewChild, Injectable, Input, Output, EventEmitter, DoCheck } from '@angular/core';
import { FormsModule, ReactiveFormsModule, FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { Http } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { Subscription } from 'rxjs/Subscription';
import { Subject } from 'rxjs/Subject';
import './rxjs-operators';
import { EnumValues } from 'enum-values';
import { MIDIService } from '../../services/midiService';
import { ProfileService } from '../../services/profileService';
import { DropdownOption, DropdownComponent } from '../../components/mdl-dropdown/mdl-dropdown.component';
import { IMIDIInputDevice, ITranslationMap, ITranslation, ShortMessage, IMIDIOutputDevice, Transformation, Profile, VirtualOutputDevice, VirtualDevice, MIDIOutputDevice, IDropdownOption, MIDIInputDevice, Translation, ChannelMessage, MessageType, TranslationFunction, InputMatchFunction, ChannelCommand } from '../../models/domainModel';
import { ProfileComponent } from '../../components/profile/profile.component';
import { TranslationComponent } from '../../components/translation/translation.component';

@Component({
    selector: 'transformation',
    templateUrl: './transformation.component.html'
})

export class TransformationComponent {
    private subscriptions: Subscription[];
    private currentTransformation: Transformation;

    @Input() set transformation(inTransformation: Transformation) {
        this.currentTransformation = inTransformation;
        this.transformationChange.emit(inTransformation);
    }
    get transformation(): Transformation {
        return this.currentTransformation;
    }

    @Output() transformationChange: EventEmitter<Transformation> = new EventEmitter<Transformation>();


    private availableInputDevices: MIDIInputDevice[];
    private availableOutputDevices: MIDIOutputDevice[];

    constructor(private midiService: MIDIService) {
        this.subscriptions.push(this.midiService.availableInputDevicesChanges
            .subscribe(data => this.availableInputDevices = data));

        this.subscriptions.push(this.midiService.availableOutputDevicesSubject
            .subscribe(data => this.availableOutputDevices = data));
    }
}