// This file will setup Knockout, require, load the components.
import ContactViewModel = require("Scripts/components/contact/Contact.viewModel");

require.config({
    baseUrl: "Scripts",
    paths: {
        knockout: "library/knockout/knockout-3.3.0",
        text: "library/require/text-2.0.7",

        // Testing frameworks
        infuse: "library/infuse",
        qunit: "library/qunit/qunit-1.17.1",
        
    },
    waitSeconds: 0
});