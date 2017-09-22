import IContactConnector = require("dataProviders/contracts/IContactConnector");
import IAjaxConnector = require("dataProviders/contracts/IAjaxConnector");
import ContactAccount = require("models/ContactAccount");
import ContactSet = require("models/ContactSet");
import Contact = require("models/Contact");

class ContactConnector implements IContactConnector {
    
    private ajaxConnector: IAjaxConnector;

// ReSharper disable once InconsistentNaming
    constructor(IAjaxConnector: IAjaxConnector) {
        this.ajaxConnector = IAjaxConnector;
    }

    public getContactAccounts(): Promise<ContactAccount[]> {
        return this.ajaxConnector.get("/ContactAccount/getAccounts");
    }

    public getContacts(): Promise<ContactSet> {
        return this.ajaxConnector.get("/Contact/GetContactSet");
    }

    public addContacts(contact: Contact): Promise<string[]> {
        var request = {
            AccountEmail: contact.accountEmail,
            FirstName: contact.firstName,
            LastName: contact.lastName,
            Email: contact.email
        }

        return this.ajaxConnector.post("/Contact/AddContacts", request);
    }
};

export = ContactConnector;
