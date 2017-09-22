define(["require", "exports", "knockout", "components/AccountList/AccountListParams", "components/ContactList/ContactListParams", "components/Button/Button.contracts"], function (require, exports, ko, AccountListParams, ContactListParams, ButtonContracts) {
    "use strict";
    var ButtonParams = ButtonContracts.ButtonParams;
    var EButtonType = ButtonContracts.EButtonType;
    var ContactMerger = (function () {
        // ReSharper disable once InconsistentNaming
        function ContactMerger(IContactConnector) {
            this.contactConnector = IContactConnector;
        }
        ContactMerger.prototype.setup = function (params) {
            var _this = this;
            this.contactSet = ko.observable();
            this.showErrorMessage = ko.observable(false);
            this.syncRequested = ko.observable(false);
            this.syncRequested.subscribe(function () {
                // Perform a sync if anybody triggers this observable
                _this.updateContactSetPromise = _this.updateContactSet();
            });
            this.setupContactList();
            this.setupAccountList();
            this.setupFetchContacts();
        };
        ContactMerger.prototype.updateContactSet = function () {
            var _this = this;
            return this.contactConnector.getContacts().then(function (contactSet) {
                _this.contactSet(contactSet);
                _this.showErrorMessage(false);
            }).catch(function (e) {
                _this.showErrorMessage(true);
                // Continue the chain
                throw e;
            });
        };
        ContactMerger.prototype.setupFetchContacts = function () {
            var _this = this;
            this.fetchContacts = new ButtonParams("Fetch Contacts", function () {
                return _this.updateContactSet();
            }, EButtonType.Primary);
        };
        ContactMerger.prototype.setupAccountList = function () {
            this.accountList = new AccountListParams();
        };
        ContactMerger.prototype.setupContactList = function () {
            var _this = this;
            this.contactList = new ContactListParams(this.contactSet, this.syncRequested);
            this.showContactList = ko.computed(function () {
                return _this.contactSet() != null;
            });
        };
        return ContactMerger;
    }());
    return ContactMerger;
});
//# sourceMappingURL=ContactMerger.viewModel.js.map