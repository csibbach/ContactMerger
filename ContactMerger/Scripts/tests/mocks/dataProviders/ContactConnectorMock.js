define(["require", "exports", "tests/utility/PromiseControls"], function (require, exports, PromiseControls) {
    "use strict";
    var ContactConnectorMock = (function () {
        function ContactConnectorMock(assert) {
            this.assert = assert;
            this.getContactAccountsControls = new PromiseControls(this.assert);
            this.getContactsControls = new PromiseControls(this.assert);
            this.addContactsControls = new PromiseControls(this.assert);
        }
        ContactConnectorMock.prototype.getContactAccounts = function () {
            return this.getContactAccountsControls.promise;
        };
        ContactConnectorMock.prototype.getContacts = function () {
            return this.getContactsControls.promise;
        };
        ContactConnectorMock.prototype.addContacts = function (contact) {
            return this.addContactsControls.promise;
        };
        return ContactConnectorMock;
    }());
    ;
    return ContactConnectorMock;
});
//# sourceMappingURL=ContactConnectorMock.js.map