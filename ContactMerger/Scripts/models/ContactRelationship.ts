import Contact = require("models/Contact");

class ContactRelationship {
    public contact: Contact;
    public contactAccountMap: string[];
}

export = ContactRelationship;