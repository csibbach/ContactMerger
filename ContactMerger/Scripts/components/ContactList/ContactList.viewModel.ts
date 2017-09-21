import ko = require("knockout");
import ContactListParams = require("components/ContactList/ContactListParams");
import ContactSet = require("models/ContactSet");

class ContactList {
    private contactSet: KnockoutObservable<ContactSet>;

    public accountNames: KnockoutComputed<Account>;

    constructor() {
        // Injection here
    }

    public setup(params: ContactListParams) {
        this.contactSet = params.contactSet;

        this.setupAccountNames();
    }

    private setupAccountNames() {
        this.accountNames = ko.computed(() => {
            return this.contactSet().accounts;
        });
    }
}

export = ContactList;