// This is a generic factory for View Models. It allows the view models to be created with
// injected parameters, via the shared singleton kernel. It provides the parameters to
// the VM via the setup function, as the main constructor is only for injection.

import kernel = require("infrastructure/Kernel");
import IViewModel = require("infrastructure/IViewModel");

class ViewModelFactory {
    
    static createViewModel(viewModelInterface: string, params: any, componentInfo: KnockoutComponentTypes.ComponentInfo): IViewModel {
        // Create the view model
        if (!kernel.hasMapping(viewModelInterface)) {
            throw new Error("No mapping exists for " + viewModelInterface + " while trying to inject the view model");
        }
        var viewModel = <IViewModel>kernel.getValue(viewModelInterface);

        // Give the parameters to the view model; the main constructor is only for injection
        viewModel.setup(params, componentInfo);

        return viewModel;
    }
}

export = ViewModelFactory;