import Account = require("models/Account");
import ContactRelationship = require("models/ContactRelationship");

class ContactSet {
    public accounts: Account[];
    public contacts: ContactRelationship[];
}

export = ContactSet;