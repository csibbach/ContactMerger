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
            var names = new Array<string>();
            
            // KO optimization; unwrap an observable before a loop
            var contactSet = this.contactSet();

            // Return an empty list if the contact set is not set
            if (contactSet == null)
                return names;

            // Yes, I'm using for-in correctly here, I want to iterate over the keys
            for (var accountName in contactSet.contactGrid) {
                if (contactSet.contactGrid.hasOwnProperty(accountName)) {
                    names.push(accountName);
                }
            }

            return names;
        });
    }
}

export = ContactList;