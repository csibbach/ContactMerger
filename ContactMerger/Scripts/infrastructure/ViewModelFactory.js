// This is a generic factory for View Models. It allows the view models to be created with
// injected parameters, via the shared singleton kernel. It provides the parameters to
// the VM via the setup function, as the main constructor is only for injection.
define(["require", "exports", "infrastructure/Kernel"], function (require, exports, kernel) {
    "use strict";
    var ViewModelFactory = (function () {
        function ViewModelFactory() {
        }
        ViewModelFactory.createViewModel = function (viewModelInterface, params, componentInfo) {
            // Create the view model
            if (!kernel.hasMapping(viewModelInterface)) {
                throw new Error("No mapping exists for " + viewModelInterface + " while trying to inject the view model");
            }
            var viewModel = kernel.getValue(viewModelInterface);
            // Give the parameters to the view model; the main constructor is only for injection
            viewModel.setup(params, componentInfo);
            return viewModel;
        };
        return ViewModelFactory;
    }());
    return ViewModelFactory;
});
//# sourceMappingURL=ViewModelFactory.js.map