// Just a basic interface for view models that defines the setup function- this
// is where you do any work with the params, the main constructor is only for injection.

interface IViewModel {
    setup(params: any, componentInfo?: KnockoutComponentTypes.ComponentInfo);
}

export = IViewModel;