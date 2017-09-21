import ko = require("knockout");
import AccountListParams = require("components/AccountList/AccountListParams");
import IContactConnector = require("dataProviders/contracts/IContactConnector");
import ContactSet = require("models/ContactSet");
import ContactListParams = require("components/ContactList/ContactListParams");

class ContactMerger {
    private contactConnector: IContactConnector;
    private contactSet: KnockoutObservable<ContactSet>;
    private syncRequested: KnockoutObservable<boolean>;

    public accountList: AccountListParams;
    public showContactList: KnockoutComputed<boolean>;
    public contactList: ContactListParams;
    public showErrorMessage: KnockoutObservable<boolean>;

    // ReSharper disable once InconsistentNaming
    constructor(IContactConnector: IContactConnector) {
        this.contactConnector = IContactConnector;
    }

    public setup(params: any) {
        this.contactSet = ko.observable<ContactSet>();
        this.showErrorMessage = ko.observable(false);
        this.syncRequested = ko.observable(false);
        this.syncRequested.subscribe(() => {
            // Perform a sync if anybody triggers this observable
            this.updateContactSet();
        });

        this.setupContactList();
        this.setupAccountList();
    }

    public fetchContacts(): Promise<void> {
        return this.updateContactSet();
    }

    private updateContactSet(): Promise<void> {
        return this.contactConnector.getContacts().then((contactSet: ContactSet) => {
            this.contactSet(contactSet);
            this.showErrorMessage(false);
        }).catch((e: any) => {
            this.showErrorMessage(true);

            // Continue the chain
            throw e;
        });
    }

    private setupAccountList() {
        this.accountList = new AccountListParams();
    }

    private setupContactList() {
        this.contactList = new ContactListParams(this.contactSet, this.syncRequested);

        this.showContactList = ko.computed<boolean>(() => {
            return this.contactSet() != null;
        });
    }
}

export = ContactMerger;