import ko = require("knockout");
import IViewModel = require("infrastructure/IViewModel");
import ButtonContracts = require("components/Button/Button.contracts");
import ButtonParams = ButtonContracts.ButtonParams;
import EButtonType = ButtonContracts.EButtonType;

class ButtonViewModel implements IViewModel {
    private action: () => Promise<any>;

    public showLoading: KnockoutObservable<boolean>;
    public buttonLabel: KnockoutObservable<string> | string;

    // These are for controlling styling. Not observables because I'm not allowing the button to change types
    public defaultType: boolean;
    public primaryType: boolean;

    public setup(params: ButtonParams) {
        this.showLoading = ko.observable(false);
        this.buttonLabel = params.label;
        this.action = params.action;

        this.defaultType = params.buttonType === EButtonType.Default;
        this.primaryType = params.buttonType === EButtonType.Primary;
    }

    public buttonClicked(): Promise<any> {
        var promise = this.action();

        // If the action returns a promise, we'll spin until it's done
        if (promise != null) {
            this.showLoading(true);

            promise.then(() => {
                this.showLoading(false);
            }).catch(() => {
                this.showLoading(false);
                });
            return promise;
        }
        return null;
    }    
}

export = ButtonViewModel;