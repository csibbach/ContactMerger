// This file works as a singleton container for the injection kernel It is just to allow easy
// sharing of a common kernel, mainly between the main setup code and the view model factory.
// It is just a quick wrapper around infuse.js that encapsulates an Injector object.

import Infuse = require("infuse");

// Setup the basic kernel. This goes into the window
class Kernel {
    kernel: Injector;

    constructor() {
        this.kernel = new Infuse.Injector();
    }

    mapClass(interfaceName: string, clazz: Function, singleton?: boolean) {
        this.kernel.mapClass(interfaceName, clazz, singleton);
    }

    mapValue(valueName: string, obj: any) {
        this.kernel.mapValue(valueName, obj);
    }

    createInstance<T>(interfaceName: string): T {
        return this.kernel.createInstance(interfaceName);
    }

    getValue(interfaceName: string): any {
        return this.kernel.getValue(interfaceName);
    }

    hasMapping(interfaceName: string): boolean {
        return this.kernel.hasMapping(interfaceName);
    }

    removeMapping(interfaceName: string) {
        this.kernel.removeMapping(interfaceName);
    }

    getMappedClass(interfaceName: string): Function {
        return this.kernel.getClass(interfaceName);
    }

    // Given a reference to the class, instantiate the class 
    instantiate<T>(clazz: Function): T {
        return this.kernel.instantiate(clazz);
    }
}

var singleton = new Kernel();
export = singleton;