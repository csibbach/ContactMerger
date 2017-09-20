import ko = require("knockout");
import AccountListParams = require("components/AccountList/AccountListParams");

class ContactMerger {
    public accountList: AccountListParams;

    constructor() {
        // Injection here
    }

    setup(params: any) {
        this.setupAccountList();
    }

    private setupAccountList() {
        this.accountList = new AccountListParams();
    }
}

export = ContactMerger;