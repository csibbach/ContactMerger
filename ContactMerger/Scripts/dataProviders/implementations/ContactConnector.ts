import IContactConnector = require("dataProviders/contracts/IContactConnector");
import IAjaxConnector = require("dataProviders/contracts/IAjaxConnector");
import ContactSet = require("models/ContactSet");

class ContactConnector implements IContactConnector {
    
    private ajaxConnector: IAjaxConnector;

// ReSharper disable once InconsistentNaming
    constructor(IAjaxConnector: IAjaxConnector) {
        this.ajaxConnector = IAjaxConnector;
    }

    public getContactAccounts(): Promise<string[]> {
        return this.ajaxConnector.get("/ContactAccount/getAccounts");
    }

    public getContacts(): Promise<ContactSet> {
        return this.ajaxConnector.get("/Contact/GetContactSet").then((response: IContactSetResponse) => {
            return new ContactSet();
        });
    }
};

interface IContactSetResponse {
    
}

export = ContactConnector;
