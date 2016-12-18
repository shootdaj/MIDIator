import { IDropdownOption } from '../../models/domainModel';

export class DropdownOption implements IDropdownOption {
	constructor(public value: any, public label: string) { }
}