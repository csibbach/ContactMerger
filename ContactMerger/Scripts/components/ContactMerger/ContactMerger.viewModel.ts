import ko = require("knockout");
import AddAccountParams = require("components/AddAccount/AddAccountParams");

class ContactMerger {
    public addAccount: AddAccountParams;

    constructor() {
        // Injection here
    }

    setup(params: any) {
        this.setupAddAccount();
    }

    private setupAddAccount() {
        this.addAccount = new AddAccountParams();
    }
}

export = ContactMerger;