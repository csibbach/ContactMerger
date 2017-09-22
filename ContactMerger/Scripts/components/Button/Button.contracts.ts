export class ButtonParams {
    constructor(public label: KnockoutObservable<string> | string,
        public action: () => Promise<any>,
    public buttonType: EButtonType) { }
}

export enum EButtonType {
    Default,
    Primary
}