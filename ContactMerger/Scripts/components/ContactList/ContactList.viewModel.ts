import ko = require("knockout");
import ContactListParams = require("components/ContactList/ContactListParams");
import ContactSet = require("models/ContactSet");
import ContactParams = require("components/Contact/ContactParams");

class ContactList {
    private contactSet: KnockoutObservable<ContactSet>;
    private syncRequested: KnockoutObservable<boolean>;

    public accountNames: KnockoutComputed<string[]>;
    public contactGrid: KnockoutComputed<ContactParams[][]>;

    public setup(params: ContactListParams) {
        this.contactSet = params.contactSet;
        this.syncRequested = params.syncRequested;

        this.setupAccountNames();
        this.setupContactGrid();
    }

    private setupContactGrid() {
        this.contactGrid = ko.computed(() => {
            // Loop through the contactGrid in the set, and rotate it from column-row to row-column indexing
            // Outer array is rows
            let returnSet = new Array<ContactParams[]>();

            // If the contactSet has not been built yet exit
            if (this.contactSet() == null) {
                return returnSet;
            }

            let contactGrid = this.contactSet().contactGrid;

            // Get the account keys into an easier array
            let accountKeys = new Array<string>();
            for (let accountKey in contactGrid) {
                if (contactGrid.hasOwnProperty(accountKey)) {
                    accountKeys.push(accountKey);
                }
            }

            // Check for an empty set just in case
            if (accountKeys.length === 0) {
                return returnSet;
            }

            // The number of rows in each column is the same, so the count of the first column will give us
            var numRows = contactGrid[accountKeys[0]].length;

            // Create an observable for if this is the only column or not. Based on the way we're regenerating this list, observable is overkill
            let notOnlyColumn = ko.observable(accountKeys.length > 1);

            // Loop through each row
            for (let i = 0; i < numRows; i++) {
                // Create an array for the row
                let rowArray = new Array<ContactParams>();

                // Loop through each column
                for (let accountKey of accountKeys) {
                    // Access the contact at position i
                    let contact = contactGrid[accountKey][i];

                    // Create ContactParams for it.
                    let params = new ContactParams(ko.observable(contact), this.syncRequested, notOnlyColumn);

                    // Put it into the row
                    rowArray.push(params);
                }

                // Add the row to the returnSet
                returnSet.push(rowArray);
            }

            return returnSet;
        });
    }
    private setupAccountNames() {
        this.accountNames = ko.computed(() => {
            var names = new Array<string>();
            
            // KO optimization; unwrap an observable before a loop
            var contactSet = this.contactSet();

            // Return an empty list if the contact set is not set
            if (contactSet == null) 
                return names;

            // Yes, I'm using for-in correctly here, I want to iterate over the keys
            for (var accountName in contactSet.contactGrid) {
                if (contactSet.contactGrid.hasOwnProperty(accountName)) {
                    names.push(accountName);
                }
            }

            return names;
        });
    }
}

export = ContactList;