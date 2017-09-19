import ko = require("knockout");
import ViewModelFactory = require("infrastructure/ViewModelFactory");
import IViewModel = require("infrastructure/IViewModel");
import kernel = require("infrastructure/Kernel");
import AddAccountViewModel = require("components/AddAccount/AddAccount.viewModel");
import ContactViewModel = require("components/Contact/Contact.viewModel");
import ContactMergerViewModel = require("components/ContactMerger/ContactMerger.viewModel");

function registerComponent(componentName: string, viewModel: Function) {
    // Register the component. Could go nuts, this should typically handled centrally but I'm not creating
    // a full framework here.
    ko.components.register(componentName,
        {
            viewModel: {
                createViewModel: (params: any, componentInfo: KnockoutComponentTypes.ComponentInfo): IViewModel => {
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