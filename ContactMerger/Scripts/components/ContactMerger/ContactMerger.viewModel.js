define(["require", "exports", "components/AddAccount/AddAccountParams"], function (require, exports, AddAccountParams) {
    "use strict";
    var ContactMerger = (function () {
        function ContactMerger() {
            // Injection here
        }
        ContactMerger.prototype.setup = function (params) {
            this.setupAddAccount();
        };
        ContactMerger.prototype.setupAddAccount = function () {
            this.addAccount = new AddAccountParams();
        };
        return ContactMerger;
    }());
    return ContactMerger;
});
//# sourceMappingURL=ContactMerger.viewModel.js.map