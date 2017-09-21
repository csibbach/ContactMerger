import ko = require("knockout");
import AccountListParams = require("components/AccountList/AccountListParams");
import IContactConnector = require("dataProviders/contracts/IContactConnector");
import ContactSet = require("models/ContactSet");
import ContactListParams = require("components/ContactList/ContactListParams");

class ContactMerger {
    private contactConnector: IContactConnector;
    private contactSet: KnockoutObservable<ContactSet>;

    public accountList: AccountListParams;
    public showContactList: KnockoutComputed<boolean>;
    public contactList: ContactListParams;

    // ReSharper disable once InconsistentNaming
    constructor(IContactConnector: IContactConnector) {
        this.contactConnector = IContactConnector;
    }

    public setup(params: any) {
        this.contactSet = ko.observable<ContactSet>();

        this.setupContactList();
        this.setupAccountList();
    }

    public fetchContacts(): Promise<void> {
        return this.contactConnector.getContacts().then((contactSet: ContactSet) => {
            this.contactSet(contactSet);
        });
    }

    private setupAccountList() {
        this.accountList = new AccountListParams();
    }

    private setupContactList() {
        this.contactList = new ContactListParams(this.contactSet);

        this.showContactList = ko.computed<boolean>(() => {
            return this.contactSet() != null;
        });
    }
}

export = ContactMerger;