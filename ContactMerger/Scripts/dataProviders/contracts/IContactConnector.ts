import ContactSet = require("models/ContactSet");
import ContactAccount = require("models/ContactAccount");

interface IContactConnector {
    getContactAccounts(): Promise<ContactAccount[]>;
    getContacts(): Promise<ContactSet>;
};

export = IContactConnector;
