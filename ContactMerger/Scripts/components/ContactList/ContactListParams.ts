import ContactSet = require("models/ContactSet");

class ContactListParams {
    constructor(public contactSet: KnockoutObservable<ContactSet>,
        public syncRequested: KnockoutObservable<boolean>) { }
}

export = ContactListParams;