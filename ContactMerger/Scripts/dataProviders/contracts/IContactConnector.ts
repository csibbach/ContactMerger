import ContactSet = require("models/ContactSet");

interface IContactConnector {
    getContactAccounts(): Promise<string[]>;
    getContacts(): Promise<ContactSet>;
};

export = IContactConnector;
