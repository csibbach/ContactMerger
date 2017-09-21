import Contact = require("models/Contact");

class ContactSet {
    public contactGrid: { [email: string]: Contact[]};
}

export = ContactSet;