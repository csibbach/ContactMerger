import ko = require("knockout");
import AddAccountParams = require("components/AddAccount/AddAccountParams");

class AddAccount {

    public showIframe: KnockoutObservable<boolean>;

    constructor() {
        // Injection here
    }

    setup(params: AddAccountParams) {
        this.showIframe = ko.observable(false);
    }

    public addAccountClicked() {
        // Create an iframe and send it to the ContactAccount/AddContactAccount endpoint.
        // When it returns, it will close the iframe, if everything goes according to plan.
        this.showIframe(true);
    }
}

export = AddAccount;