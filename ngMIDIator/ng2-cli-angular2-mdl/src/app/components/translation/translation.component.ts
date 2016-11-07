//domain model
import { IMIDIInputDevice, ITranslationMap, ITranslation, ShortMessage, IMIDIOutputDevice, Transformation, Profile, VirtualOutputDevice, VirtualDevice, MIDIOutputDevice, IDropdownOption, MIDIInputDevice, Translation, ChannelMessage, MessageType, TranslationFunction, InputMatchFunction, ChannelCommand } from '../../models/domainModel';

//services
import { MIDIService } from '../../services/midiService';
import { ProfileService } from '../../services/profileService';

//components
import { DropdownOption, DropdownComponent } from '../../components/mdl-dropdown/mdl-dropdown.component';
import { TransformationComponent } from '../../components/transformation/transformation.component';
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
	selector: 'translation',
	templateUrl: './translation.component.html',
	providers: [MIDIService]
})

export class TranslationComponent {

    //private currentForm: FormGroup;
    //@Output() formChanges: EventEmitter<Translation> = new EventEmitter<Translation>();

    //@Input() set form(form: FormGroup) {
    //    this.currentForm = form;
    //    this.currentFormChange.emit(form);
    //}
    //get form() {
    //    return this.currentForm;
    //}

    @Input() public form: FormGroup;
   




	//private subscriptions: Subscription[];
	//private availableInputDevices: MIDIInputDevice[];
	//private availableOutputDevices: MIDIOutputDevice[];
	//private availableInputMatchFunctions: InputMatchFunction[];
	//private availableTranslationFunctions: TranslationFunction[];
	
	////@Input() translation: Translation;
	////@Output() translationChange: EventEmitter<Translation> = new EventEmitter<Translation>();

 //   constructor(private midiService: MIDIService, private fb: FormBuilder) {

 //       //this.form = this.fb.group({
            
 //       //    addresses: this.fb.array([
 //       //        this.initAddress(),
 //       //    ])
 //       //});

	//	this.subscriptions.push(this.midiService.availableInputDevicesChanges
	//		.subscribe(data => this.availableInputDevices = data));

	//	this.subscriptions.push(this.midiService.availableOutputDevicesSubject
	//		.subscribe(data => this.availableOutputDevices = data));

	//	this.subscriptions.push(this.midiService.availableInputMatchFunctionsSubject
	//		.subscribe(data => this.availableInputMatchFunctions = data));

	//	this.subscriptions.push(this.midiService.availableTranslationFunctionsSubject
 //           .subscribe(data => this.availableTranslationFunctions = data));

	//    this.form.valueChanges.subscribe(data => this.formChanges.emit(data));

	//}

	//ngOnDestroy() {
	//	this.subscriptions.forEach(s => s.unsubscribe());
	//}

	//get availableInputMatchFunctionDropdownOptions(): IDropdownOption[] {
	//	return this.availableInputMatchFunctions.map(
	//		fx => new DropdownOption((<number>fx).toString(), InputMatchFunction[fx]));
	//}

	//get availableTranslationFunctionDropdownOptions(): IDropdownOption[] {
	//	return this.availableTranslationFunctions.map(
	//		fx => new DropdownOption((<number>fx).toString(), TranslationFunction[fx]));
	//}

	//get inputMatchFunctionDropdownOption(): IDropdownOption {
	//	return new DropdownOption((<number>this.form.value.inputMatchFunction).toString(), InputMatchFunction[this.translation.inputMatchFunction]);
	//}
	//set inputMatchFunctionDropdownOption(value: IDropdownOption) {
	//	this.translation.inputMatchFunction = parseInt(value.value);
	//}

	//get translationFunctionDropdownOption(): IDropdownOption {
	//	return new DropdownOption((<number>this.translation.inputMatchFunction).toString(), InputMatchFunction[this.translation.inputMatchFunction]);
	//}
	//set translationFunctionDropdownOption(value: IDropdownOption) {
	//	this.translation.translationFunction = parseInt(value.value);
	//}
}