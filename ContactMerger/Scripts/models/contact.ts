import ko = require("knockout");

class Contact {
    public firstName: KnockoutObservable<string>;
    public lastName: KnockoutObservable<string>;
    public email: KnockoutObservable<string>;
}

export = Contact;