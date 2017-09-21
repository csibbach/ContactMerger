import ko = require("knockout");
import AccountListParams = require("components/AccountList/AccountListParams");
import IContactConnector = require("dataProviders/contracts/IContactConnector");

class AccountList {
    private contactConnector: IContactConnector;
    public accountNames: KnockoutObservableArray<string>;

    // ReSharper disable InconsistentNaming
    constructor(IContactConnector: IContactConnector) {
        // ReSharper restore InconsistentNaming
        this.contactConnector = IContactConnector;
    }

    public setup(params: AccountListParams) {
        this.accountNames = ko.observableArray<string>();

        // Get the list of account names and show them when they are loaded.
        this.contactConnector.getContactAccounts().then((accountNames: string[]) => {
                this.accountNames(accountNames);
            })
            .catch(() => {
                this.accountNames(["Could not retrieve accounts!"]);
            });
    }
}

export = AccountList;