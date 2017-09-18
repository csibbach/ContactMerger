// This file works as a singleton container for the injection kernel It is just to allow easy
// sharing of a common kernel, mainly between the main setup code and the view model factory.
// It is just a quick wrapper around infuse.js that encapsulates an Injector object.
define(["require", "exports", "infuse"], function (require, exports, Infuse) {
    "use strict";
    // Setup the basic kernel. This goes into the window
    var Kernel = (function () {
        function Kernel() {
            this.kernel = new Infuse.Injector();
        }
        Kernel.prototype.mapClass = function (interfaceName, clazz, singleton) {
            this.kernel.mapClass(interfaceName, clazz, singleton);
        };
        Kernel.prototype.mapValue = function (valueName, obj) {
            this.kernel.mapValue(valueName, obj);
        };
        Kernel.prototype.createInstance = function (interfaceName) {
            return this.kernel.createInstance(interfaceName);
        };
        Kernel.prototype.getValue = function (interfaceName) {
            return this.kernel.getValue(interfaceName);
        };
        Kernel.prototype.hasMapping = function (interfaceName) {
            return this.kernel.hasMapping(interfaceName);
        };
        Kernel.prototype.removeMapping = function (interfaceName) {
            this.kernel.removeMapping(interfaceName);
        };
        Kernel.prototype.getMappedClass = function (interfaceName) {
            return this.kernel.getClass(interfaceName);
        };
        // Given a reference to the class, instantiate the class 
        Kernel.prototype.instantiate = function (clazz) {
            return this.kernel.instantiate(clazz);
        };
        return Kernel;
    }());
    var singleton = new Kernel();
    return singleton;
});
//# sourceMappingURL=Kernel.js.map