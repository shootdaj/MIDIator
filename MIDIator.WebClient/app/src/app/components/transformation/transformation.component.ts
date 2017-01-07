import { Component, ViewChild, Injectable, Input, Output, EventEmitter, DoCheck, OnInit, AfterViewInit, OnDestroy, trigger, state, style, transition, animate } from '@angular/core';
import { FormsModule, FormArray, ReactiveFormsModule, FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { Http } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { Subscription } from 'rxjs/Subscription';
import { Subject } from 'rxjs/Subject';
import '../../rxjs-operators';
import { EnumValues } from 'enum-values';
import { MIDIService } from '../../services/midiService';
import { HelperService } from '../../services/helperService';
import { ProfileService } from '../../services/profileService';
import { FormService } from '../../services/formService';
import { DropdownOption } from '../../components/mdl-dropdown/dropdownOption';
import { DropdownComponent } from '../../components/mdl-dropdown/mdl-dropdown.component';
import { IMIDIInputDevice, ShortMessage, IMIDIOutputDevice, Transformation, Profile, VirtualOutputDevice, VirtualDevice, MIDIOutputDevice, IDropdownOption, MIDIInputDevice, Translation, ChannelMessage, MessageType, TranslationFunction, InputMatchFunction, ChannelCommand } from '../../models/domainModel';
import { ProfileComponent } from '../../components/profile/profile.component';
import { TranslationComponent } from '../../components/translation/translation.component';

@Component({
    selector: 'transformation',
    templateUrl: './transformation.component.html',
    styles: ['.adjust-right-alignment { margin-right: 16px;}']
})

export class TransformationComponent implements OnInit, OnDestroy {

    private subscriptions: Subscription[];
    private inputDevices: MIDIInputDevice[];
    private outputDevices: MIDIOutputDevice[];

    @Input() form: FormGroup;
    @Output() deleteTransformationChange = new EventEmitter();

    constructor(private midiService: MIDIService,
        private helperService: HelperService,
        private formService: FormService) {
    }

    ngOnInit(): void {
        this.subscriptions = new Array<Subscription>();
        this.subscriptions.push(this.midiService.availableInputDevicesChanges
            .subscribe(data => {
                this.inputDevices = data.map(device => this.helperService.maskCast(device, MIDIInputDevice));
            }));
        this.subscriptions.push(this.midiService.availableOutputDevicesChanges
            .subscribe(data => {
                this.outputDevices = data.map(device => this.helperService.maskCast(device, MIDIOutputDevice));
            }));

        this.midiService.getAvailableInputDevices();
        this.midiService.getAvailableOutputDevices();
    }

    ngOnDestroy(): void {
        this.subscriptions.forEach(s => s.unsubscribe());
    }

    private linkOutputDevice() {
        this.form.controls['linkedOutputVirtualDevice'].setValue(true);
    }

    private unlinkOutputDevice() {
        this.form.controls['linkedOutputVirtualDevice'].setValue(false);
    }

    private get linkedOutputDevice() {
        return this.form.controls['linkedOutputVirtualDevice'].value;
    }

    private addNewTranslation() {
        let control = <FormArray>(<FormGroup>this.form.controls['translationMap']).controls['translations'];
        control.push(this.initTranslationFormGroup());
    }

    private initTranslationFormGroup() {
        return this.formService.getTranslationFormGroup(this.helperService.initTranslation());
    }

    private deleteTransformation() {
        this.deleteTransformationChange.next(this.form.value.name);
    }

    private deleteTranslation(index) {
        let control = <FormArray>((<FormArray>this.form.controls['translationMap']).controls['translations']);
        control.removeAt(index);
    }
}