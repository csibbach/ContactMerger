import ko = require("knockout");
import AccountListParams = require("components/AccountList/AccountListParams");
import IContactAccountConnector = require("dataProviders/contracts/IContactAccountConnector");

class AccountList {
    private contactAccountConnector: IContactAccountConnector;
    public accountNames: KnockoutObservableArray<string>;

    // ReSharper disable InconsistentNaming
    constructor(IContactAccountConnector: IContactAccountConnector) {
        // ReSharper restore InconsistentNaming
        this.contactAccountConnector = IContactAccountConnector;
    }

    public setup(params: AccountListParams) {
        this.accountNames = ko.observableArray<string>();

        // Get the list of account names and show them when they are loaded.
        this.contactAccountConnector.getContactAccounts().then((accountNames: string[]) => {
                this.accountNames(accountNames);
            })
            .catch(() => {
                this.accountNames(["Could not retrieve accounts!"]);
            });
    }
}

export = AccountList;