import Contact = require("models/Contact");

class ContactParams {
    constructor(public contact: KnockoutObservable<Contact>,
        public syncRequested: KnockoutObservable<boolean>,
        public notOnlyColumn: KnockoutObservable<boolean>) {
    }
}

export = ContactParams;