import $ = require("jquery");
import IContactAccountConnector = require("dataProviders/contracts/IContactAccountConnector");

class ContactAccountConnector implements IContactAccountConnector {
    public getContactAccounts(): Promise<string[]> {
        return Promise.resolve($.get("/ContactAccount/getAccounts"));
    }
};

export = ContactAccountConnector;
