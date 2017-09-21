import ko = require("knockout");
import ContactParams = require("components/Contact/ContactParams");
import ContactModel = require("models/Contact");
import IContactConnector = require("dataProviders/contracts/IContactConnector");

class ContactViewModel {
    public name: KnockoutComputed<string>;
    public nameMatches: KnockoutComputed<boolean>;
    public email: KnockoutComputed<string>;
    public emailMatches: KnockoutComputed<boolean>;
    public contactExists: KnockoutComputed<boolean>;
    public showErrorMessage: KnockoutObservable<boolean>;
    public notOnlyColumn: KnockoutObservable<boolean>;
    
    private contact: KnockoutObservable<ContactModel>;
    private contactConnector: IContactConnector;
    private syncRequested: KnockoutObservable<boolean>;

    // ReSharper disable once InconsistentNaming
    constructor(IContactConnector: IContactConnector) {
        this.contactConnector = IContactConnector;
    }

    public setup(params: ContactParams) {
        this.contact = params.contact;
        this.syncRequested = params.syncRequested;
        this.notOnlyColumn = params.notOnlyColumn;

        this.showErrorMessage = ko.observable(false);

        this.name = ko.computed(() => {
            let contact = this.contact();
            if (contact.firstName === "" && contact.lastName === "") {
                return "No Name";
            }

            // Yeah, last name only will have a leading space...
            return `${contact.firstName} ${contact.lastName}`;
        });

        this.email = ko.computed(() => {
            let contact = this.contact();
            if (contact.email === "") {
                return "No email";
            }
            return contact.email;
        });

        this.nameMatches = ko.computed(() => {
            return this.contact().firstNameMatches && this.contact().lastNameMatches;
        });

        this.emailMatches = ko.computed(() => {
            return this.contact().emailMatches;
        });

        this.contactExists = ko.computed(() => {
            return this.contact().contactExists;
        });

    }

    // Returns the promise so that it's easier to test.
    public syncContact(): Promise<void> {
        // This function will tell the app to use this contact's data in the others on this row
        alert("sync requested");

        return Promise.resolve().then(() => {
            // This is a handy way to make an observable work like a dedicated signal object. I prefer the latter but
            // it would be one more thing to bring in.
            this.syncRequested.valueHasMutated();

            this.showErrorMessage(false);
        }).catch((e: any) => {
            this.showErrorMessage(true);

            // Mark the promise as having failed.
            throw e;
        });
    }
}

export = ContactViewModel;
