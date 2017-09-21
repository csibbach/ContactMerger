import ko = require("knockout");
import ViewModelFactory = require("infrastructure/ViewModelFactory");
import IViewModel = require("infrastructure/IViewModel");
import kernel = require("infrastructure/Kernel");
import AccountListViewModel = require("components/AccountList/AccountList.viewModel");
import ContactLineItemViewModel = require("components/ContactLineItem/ContactLineItem.viewModel");
import ContactListViewModel = require("components/ContactList/ContactList.viewModel");
import ContactMergerViewModel = require("components/ContactMerger/ContactMerger.viewModel");
import ContactConnector = require("dataProviders/implementations/ContactConnector");
import JQueryAjaxConnector = require("dataProviders/implementations/JQueryAjaxConnector");

function registerComponent(componentName: string, viewModel: Function) {
    // Register the component. Could go nuts, this should typically handled centrally but I'm not creating
    // a full framework here.
    ko.components.register(componentName,
        {
            viewModel: {
                createViewModel: (params: any, componentInfo: KnockoutComponentTypes.ComponentInfo): IViewModel => {
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
registerComponent("ContactLineItem", ContactLineItemViewModel);
registerComponent("ContactMerger", ContactMergerViewModel);
registerComponent("ContactList", ContactListViewModel);

// Bind engines and data providers and whatever else
kernel.mapClass("IAjaxConnector", JQueryAjaxConnector);
kernel.mapClass("IContactConnector", ContactConnector);

// Kick off knockout to do its thang...
ko.applyBindings();