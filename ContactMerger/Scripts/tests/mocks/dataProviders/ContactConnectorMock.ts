﻿import IContactConnector = require("dataProviders/contracts/IContactConnector");
import PromiseControls = require("tests/utility/PromiseControls");
import ContactSet = require("models/ContactSet");
import ContactAccount = require("models/ContactAccount");
import Contact = require("models/Contact");

class ContactConnectorMock implements IContactConnector {
    public constructor(private assert: QUnitAssert) {}

    public getContactAccountsControls = new PromiseControls<ContactAccount[]>(this.assert);
    public getContactAccounts(): Promise<ContactAccount[]> {
        return this.getContactAccountsControls.promise;
    }

    public getContactsControls = new PromiseControls<ContactSet>(this.assert);
    public getContacts(): Promise<ContactSet> {
        return this.getContactsControls.promise;
    }

    public addContactsControls = new PromiseControls<string[]>(this.assert);
    public addContacts(contact: Contact): Promise<string[]> {
        return this.addContactsControls.promise;
    }
};

export = ContactConnectorMock;
