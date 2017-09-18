import ContactList = require("models/Contact");
import Account = require("models/Account");

interface IContactProvider {
    getContacts(account: Account): ContactList;

};

export = IContactProvider;