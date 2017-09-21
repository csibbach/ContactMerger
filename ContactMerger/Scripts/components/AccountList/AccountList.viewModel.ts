import ko = require("knockout");
import AccountListParams = require("components/AccountList/AccountListParams");
import IContactConnector = require("dataProviders/contracts/IContactConnector");
import ContactAccount = require("models/ContactAccount");
import EAccountType = require("enum/EAccountType");

class AccountList {
    private contactConnector: IContactConnector;
    private contactAccounts: KnockoutObservableArray<ContactAccount>;

    public accountNames: KnockoutComputed<AccountLineItem[]>;
    public showErrorMessage: KnockoutObservable<boolean>;

    // ReSharper disable InconsistentNaming
    constructor(IContactConnector: IContactConnector) {
        // ReSharper restore InconsistentNaming
        this.contactConnector = IContactConnector;
    }

    public setup(params: AccountListParams) {
        this.contactAccounts = ko.observableArray<ContactAccount>();
        this.showErrorMessage = ko.observable(false);

        this.setupAccountNames();

        // Get the list of account names and show them when they are loaded.
        this.contactConnector.getContactAccounts().then((contactAccounts: ContactAccount[]) => {
            this.contactAccounts(contactAccounts);
                this.showErrorMessage(false);
            })
            .catch(() => {
                this.showErrorMessage(true);
            });
    }

    private setupAccountNames() {
        this.accountNames = ko.computed(() => {
            var list = new Array<AccountLineItem>();

            // Loop over the contactAccounts and convert them to line items.
            var contactAccounts = this.contactAccounts();
            for (var contactAccount of contactAccounts) {
                list.push(new AccountLineItem(contactAccount));
            }

            return list;
        });
    }
}

class AccountLineItem {
    public accountName: string;
    public showGoogleIcon: KnockoutObservable<boolean>;
    public showFacebookIcon: KnockoutObservable<boolean>;

    constructor(contactAccount: ContactAccount) {
        this.accountName = contactAccount.accountEmail;
        this.showGoogleIcon = ko.observable(contactAccount.contactAccountType === EAccountType.Google);
        this.showFacebookIcon = ko.observable(contactAccount.contactAccountType === EAccountType.Facebook);
    }
}

export = AccountList;