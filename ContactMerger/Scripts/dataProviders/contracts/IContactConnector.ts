import ContactList = require("models/Contact");
import Account = require("models/Account");

interface IContactConnector {
    getContacts(account: Account): ContactList;

};

export = IContactConnector;