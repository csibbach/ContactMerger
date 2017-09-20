import IContactAccountConnector = require("dataProviders/contracts/IContactAccountConnector");
import PromiseControls = require("tests/utility/PromiseControls");

class ContactAccountConnectorMock implements IContactAccountConnector {
    public constructor(private assert: QUnitAssert) {}

    public getContactAccountsPromise = new PromiseControls<string[]>(this.assert);
    public getContactAccounts(): Promise<string[]> {
        return this.getContactAccountsPromise.promise;
    }
};

export = ContactAccountConnectorMock;
