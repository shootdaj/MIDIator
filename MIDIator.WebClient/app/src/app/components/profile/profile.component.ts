import { Component, ViewChild, Injectable, Input, Output, EventEmitter, OnInit, AfterViewInit, OnDestroy, ChangeDetectionStrategy } from '@angular/core';
import { FormArray, FormsModule, ReactiveFormsModule, FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { Http } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { Subscription } from 'rxjs/Subscription';
import { Subject } from 'rxjs/Subject';
import '../../rxjs-operators';
import { EnumValues } from 'enum-values';
import { MIDIService } from '../../services/midiService';
import { ProfileService } from '../../services/profileService';
import { FormService } from '../../services/formService';
import { HelperService } from '../../services/helperService';
import { RealtimeService } from '../../services/realtimeService';
import { DropdownComponent } from '../../components/mdl-dropdown/mdl-dropdown.component';
import { DropdownOption } from '../../components/mdl-dropdown/dropdownOption';
import { IMIDIInputDevice, ShortMessage, IMIDIOutputDevice, Transformation, Profile, VirtualOutputDevice, VirtualDevice, MIDIOutputDevice, IDropdownOption, MIDIInputDevice, Translation, ChannelMessage, MessageType, TranslationFunction, InputMatchFunction, ChannelCommand, TranslationMap } from '../../models/domainModel';
import { TransformationComponent } from '../../components/transformation/transformation.component';
import { TranslationComponent } from '../../components/translation/translation.component';
import { TextInputComponent } from '../../components/mdl-textinput/mdl-textinput.component';

@Component({
    selector: 'profile',
    templateUrl: './profile.component.html',
    styles: ['.adjust-right-alignment {margin-right: 5px;}']
})

export class ProfileComponent {

    @Input() form: FormGroup;

    constructor(private realtimeService: RealtimeService,
        private midiService: MIDIService,
        private formService: FormService,
        private helperService: HelperService) {
    }

    private isRealtimeEnabled(): boolean {
        return this.realtimeService.isRealtimeEnabled();
    }

    enableRealtime() {
        this.realtimeService.enableRealtime();
    }

    disableRealtime() {
        this.realtimeService.disableRealtime();
    }

    addNewTransformation() {
        let control = <FormArray>this.form.controls['transformations'];
        control.push(this.initTransformation("Transformation" +
            ((<FormArray>this.form.controls['transformations']).controls.length + 1)));
    }

    private initTransformation(name: string) {
        let transformation = this.helperService.initTransformation(name);
        return this.formService.getTransformationsFormGroups(<Transformation[]>[transformation])[0];
    }


    private deleteTransformation(name) {
        let control = <FormArray>this.form.controls['transformations'];
        
        for (var i = 0; i < control.length; i++) {
            if (control.controls[i].value.name === name) {
                control.removeAt(i);
            }
        }
    }
}