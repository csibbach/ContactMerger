define(["require", "exports", "knockout", "infrastructure/ViewModelFactory", "infrastructure/Kernel", "components/AddAccount/AddAccount.viewModel", "components/Contact/Contact.viewModel", "components/ContactMerger/ContactMerger.viewModel"], function (require, exports, ko, ViewModelFactory, kernel, AddAccountViewModel, ContactViewModel, ContactMergerViewModel) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    function registerComponent(componentName, viewModel) {
        // Register the component. Could go nuts, this should typically handled centrally but I'm not creating
        // a full framework here.
        ko.components.register(componentName, {
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
        kernel.mapClass(componentName, viewModel);
    }
    // Register all the components we're going to use
    registerComponent("AddAccount", AddAccountViewModel);
    registerComponent("Contact", ContactViewModel);
    registerComponent("ContactMerger", ContactMergerViewModel);
    // Bind engines and data providers and whatever else
    // Kick off knockout to do its thang...
    ko.applyBindings();
});
//# sourceMappingURL=main.js.map