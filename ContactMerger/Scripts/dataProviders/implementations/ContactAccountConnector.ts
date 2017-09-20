import $ = require("jquery");
import AccountConnector = require("dataProviders/contracts/IContactAccountConnector");

class ContactAccountConnector implements AccountConnector {
    public getContactAccounts(): Promise<string[]> {
        return Promise.resolve($.get("/ContactAccount/getAccounts"));
    }
};

export = ContactAccountConnector;
