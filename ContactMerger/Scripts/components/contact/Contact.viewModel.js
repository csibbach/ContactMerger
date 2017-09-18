define(["require", "exports", "knockout", "infrastructure/ViewModelFactory"], function (require, exports, ko, ViewModelFactory) {
    "use strict";
    var Contact = (function () {
        function Contact() {
        }
        return Contact;
    }());
    // Register the component. Could go nuts, this should typically handled centrally but I'm not creating
    // a full framework here.
    ko.components.register(this.uniqueName, {
        viewModel: {
            createViewModel: function (params, componentInfo) {
                return ViewModelFactory.createViewModel("Contact", params, componentInfo);
            }
        },
        template: {
            require: "text!components/Contact/Contact.template.html"
        }
    });
    return Contact;
});
//# sourceMappingURL=Contact.viewModel.js.map