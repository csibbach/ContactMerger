import ko = require("knockout");
import ContactLineItemParams = require("components/ContactLineItem/ContactLineItemParams");

class ContactLineItem {
    public firstName: string;
    public lastName: string;
    public email: string;

    constructor() {
        // Injection here
    }

    setup(params: ContactLineItemParams) {

    }
}

export = ContactLineItem;