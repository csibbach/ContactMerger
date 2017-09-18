import ko = require("knockout");
import ContactParams = require("components/Contact/ContactParams");
import ViewModelFactory = require("infrastructure/ViewModelFactory");
import IViewModel = require("infrastructure/IViewModel");

class Contact {
    public firstName: string;
    public lastName: string;
    public email: string;
}

// Register the component. Could go nuts, this should typically handled centrally but I'm not creating
// a full framework here.
ko.components.register(this.uniqueName,
    {
        viewModel: {
            createViewModel: (params: any, componentInfo: KnockoutComponentTypes.ComponentInfo): IViewModel => {
                return ViewModelFactory.createViewModel("Contact", params, componentInfo);
            }
        },
        template: {
            require: "text!components/Contact/Contact.template.html"
        }
    });
export = Contact;