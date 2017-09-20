define(["require", "exports", "tests/utility/PromiseControls"], function (require, exports, PromiseControls) {
    "use strict";
    var ContactAccountConnectorMock = (function () {
        function ContactAccountConnectorMock(assert) {
            this.assert = assert;
            this.getContactAccountsPromise = new PromiseControls(this.assert);
        }
        ContactAccountConnectorMock.prototype.getContactAccounts = function () {
            return this.getContactAccountsPromise.promise;
        };
        return ContactAccountConnectorMock;
    }());
    ;
    return ContactAccountConnectorMock;
});
//# sourceMappingURL=ContactAccountConnectorMock.js.map