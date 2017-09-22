import ContactSet = require("models/ContactSet");
import ContactAccount = require("models/ContactAccount");
import Contact = require("models/Contact");

interface IContactConnector {
    getContactAccounts(): Promise<ContactAccount[]>;
    getContacts(): Promise<ContactSet>;
    addContacts(contact: Contact): Promise<string[]>;
};

export = IContactConnector;
