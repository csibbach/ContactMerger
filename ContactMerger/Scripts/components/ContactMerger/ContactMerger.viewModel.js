define(["require", "exports", "knockout", "components/AccountList/AccountListParams", "components/ContactList/ContactListParams"], function (require, exports, ko, AccountListParams, ContactListParams) {
    "use strict";
    var ContactMerger = (function () {
        // ReSharper disable once InconsistentNaming
        function ContactMerger(IContactConnector) {
            this.contactConnector = IContactConnector;
        }
        ContactMerger.prototype.setup = function (params) {
            this.contactSet = ko.observable();
            this.setupContactList();
            this.setupAccountList();
        };
        ContactMerger.prototype.fetchContacts = function () {
            var _this = this;
            return this.contactConnector.getContacts().then(function (contactSet) {
                _this.contactSet(contactSet);
            });
        };
        ContactMerger.prototype.setupAccountList = function () {
            this.accountList = new AccountListParams();
        };
        ContactMerger.prototype.setupContactList = function () {
            var _this = this;
            this.contactList = new ContactListParams(this.contactSet);
            this.showContactList = ko.computed(function () {
                return _this.contactSet() != null;
            });
        };
        return ContactMerger;
    }());
    return ContactMerger;
});
//# sourceMappingURL=ContactMerger.viewModel.js.map