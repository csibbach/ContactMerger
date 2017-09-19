define(["require", "exports", "knockout", "infrastructure/ViewModelFactory", "infrastructure/Kernel"], function (require, exports, ko, ViewModelFactory, kernel) {
    "use strict";
    var AddAccount = (function () {
        function AddAccount() {
            // Injection here
        }
        AddAccount.prototype.setup = function (params) {
        };
        return AddAccount;
    }());
    // Register the component. Could go nuts, this should typically handled centrally but I'm not creating
    // a full framework here.
    ko.components.register(this.uniqueName, {
        viewModel: {
            createViewModel: function (params, componentInfo) {
                return ViewModelFactory.createViewModel("AddAccount", params, componentInfo);
            }
        },
        template: {
            require: "text!components/AddAccount/AddAccount.template.html"
        }
    });
    // For injection, we need to map this viewmodel to this name.
    kernel.mapClass("AddAccount", AddAccount);
    return AddAccount;
});
//# sourceMappingURL=AddAccout.viewModel.js.map