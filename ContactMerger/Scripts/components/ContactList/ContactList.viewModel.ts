import ko = require("knockout");
import ContactListParams = require("components/ContactList/ContactListParams");
import ContactSet = require("models/ContactSet");

class ContactList {
    private contactSet: ContactSet;

    constructor() {
        // Injection here
    }

    public setup(params: ContactListParams) {
        this.contactSet = params.contactSet;
    }
}

export = ContactList;