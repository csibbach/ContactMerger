define(["require", "exports", "components/AccountList/AccountListParams"], function (require, exports, AccountListParams) {
    "use strict";
    var ContactMerger = (function () {
        function ContactMerger() {
            // Injection here
        }
        ContactMerger.prototype.setup = function (params) {
            this.setupAccountList();
        };
        ContactMerger.prototype.setupAccountList = function () {
            this.accountList = new AccountListParams();
        };
        return ContactMerger;
    }());
    return ContactMerger;
});
//# sourceMappingURL=ContactMerger.viewModel.js.map