import ko = require("knockout");
import AccountListParams = require("components/AccountList/AccountListParams");
import IContactConnector = require("dataProviders/contracts/IContactConnector");
import ContactAccount = require("models/ContactAccount");
import EAccountType = require("enum/EAccountType");
import ButtonContracts = require("components/Button/Button.contracts");
import ButtonParams = ButtonContracts.ButtonParams;
import EButtonType = ButtonContracts.EButtonType;

class AccountList {
    private contactConnector: IContactConnector;
    private contactAccounts: KnockoutObservableArray<ContactAccount>;

    public accountNames: KnockoutComputed<AccountLineItem[]>;
    public showErrorMessage: KnockoutObservable<boolean>;
    public addGoogleAccount: ButtonParams;

    // Pattern issue- how to solve setup() doing something async and test it. More advanced framework would allow setup
    // to return this status directly. I'm just going to put the promise in a public spot
    public setupTestPromise: Promise<void>;

    // ReSharper disable InconsistentNaming
    constructor(IContactConnector: IContactConnector) {
        // ReSharper restore InconsistentNaming
        this.contactConnector = IContactConnector;
    }

    public setup(params: AccountListParams) {
        this.contactAccounts = ko.observableArray<ContactAccount>();
        this.showErrorMessage = ko.observable(false);

        this.setupAccountNames();
        this.setupAddGoogleAccount();

        // Get the list of account names and show them when they are loaded.
        this.setupTestPromise = this.contactConnector.getContactAccounts().then((contactAccounts: ContactAccount[]) => {
            this.contactAccounts(contactAccounts);
                this.showErrorMessage(false);
            })
            .catch(() => {
                this.showErrorMessage(true);
            });
    }

    private setupAddGoogleAccount() {
        this.addGoogleAccount = new ButtonParams("Add Google Account",
            () => {
                window.location.href = "/ContactAccount/AddGoogleContactAccount";
                return null;
            },
            EButtonType.Default);
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