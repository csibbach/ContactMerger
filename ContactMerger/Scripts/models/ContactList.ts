import Contact = require("models/Contact");
import Account = require("models/Account");

class ContactList {
    account: Account;
    contacts: KnockoutObservableArray<Contact>;
}

export = ContactList;