import { UIControlBase } from './ui-control-base';

export class TextboxQuestion extends UIControlBase<string> {
    controlType = 'textbox';
    type: string;

    constructor(options: {} = {}) {
        super(options);
        this.type = options['type'] || '';
    }
}