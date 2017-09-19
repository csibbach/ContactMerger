define(["require", "exports", "knockout"], function (require, exports, ko) {
    "use strict";
    var AddAccount = (function () {
        function AddAccount() {
            // Injection here
        }
        AddAccount.prototype.setup = function (params) {
            this.showIframe = ko.observable(false);
        };
        AddAccount.prototype.addAccountClicked = function () {
            // Create an iframe and send it to the ContactAccount/AddContactAccount endpoint.
            // When it returns, it will close the iframe, if everything goes according to plan.
            this.showIframe(true);
        };
        return AddAccount;
    }());
    return AddAccount;
});
//# sourceMappingURL=AddAccount.viewModel.js.map