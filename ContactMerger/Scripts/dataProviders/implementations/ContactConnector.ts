import IContactConnector = require("dataProviders/contracts/IContactConnector");
import IAjaxConnector = require("dataProviders/contracts/IAjaxConnector");
import ContactAccount = require("models/ContactAccount");
import ContactSet = require("models/ContactSet");

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
};

interface IContactSetResponse {
    
}

export = ContactConnector;
