import ContactSet = require("models/ContactSet");

class ContactListParams {
    constructor(public contactSet: KnockoutObservable<ContactSet>) {}
}

export = ContactListParams;