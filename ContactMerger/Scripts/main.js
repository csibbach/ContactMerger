define(["require", "exports", "knockout", "infrastructure/ViewModelFactory", "infrastructure/Kernel", "components/AccountList/AccountList.viewModel", "components/Contact/Contact.viewModel", "components/ContactList/ContactList.viewModel", "components/ContactMerger/ContactMerger.viewModel", "dataProviders/implementations/ContactConnector", "dataProviders/implementations/JQueryAjaxConnector"], function (require, exports, ko, ViewModelFactory, kernel, AccountListViewModel, ContactViewModel, ContactListViewModel, ContactMergerViewModel, ContactConnector, JQueryAjaxConnector) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    function registerComponent(componentName, viewModel) {
        // Register the component. Could go nuts, this should typically handled centrally but I'm not creating
        // a full framework here.
        ko.components.register(componentName, {
            viewModel: {
                createViewModel: function (params, componentInfo) {
                    return ViewModelFactory.createViewModel(componentName, params, componentInfo);
                }
            },
            template: {
                require: "text!components/" + componentName + "/" + componentName + ".template.html"
            }
        });
        // For injection, we need to map this viewmodel to this name.
        kernel.mapClass(componentName, viewModel);
    }
    // Register all the components we're going to use
    registerComponent("AccountList", AccountListViewModel);
    registerComponent("Contact", ContactViewModel);
    registerComponent("ContactMerger", ContactMergerViewModel);
    registerComponent("ContactList", ContactListViewModel);
    // Bind engines and data providers and whatever else
    kernel.mapClass("IAjaxConnector", JQueryAjaxConnector);
    kernel.mapClass("IContactConnector", ContactConnector);
    // Kick off knockout to do its thang...
    ko.applyBindings();
});
//# sourceMappingURL=main.js.map