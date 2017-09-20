define(["require", "exports"], function (require, exports) {
    "use strict";
    var PromiseControls = (function () {
        function PromiseControls(assert) {
            var _this = this;
            this.assert = assert;
            this.resolveState = null;
            this.promise = new Promise(function (resolve, reject) {
                _this.resolveFunction = resolve;
                _this.rejectFunction = reject;
                if (_this.resolveState == null) {
                }
                else if (_this.resolveState) {
                    resolve(_this.resolveValue);
                    _this.done();
                }
                else {
                    reject(_this.rejectValue);
                    _this.done();
                }
            });
        }
        PromiseControls.prototype.resolve = function (resolution) {
            if (resolution === void 0) { resolution = null; }
            this.done = this.assert.async();
            if (this.resolveFunction != null) {
                this.resolveFunction(resolution);
                this.done();
            }
            else {
                this.resolveState = true;
                this.resolveValue = resolution;
            }
        };
        PromiseControls.prototype.reject = function (error) {
            if (error === void 0) { error = new Error(); }
            this.done = this.assert.async();
            if (this.rejectFunction != null) {
                this.rejectFunction(error);
                this.done();
            }
            else {
                this.resolveState = false;
                this.rejectValue = error;
            }
        };
        return PromiseControls;
    }());
    return PromiseControls;
});
//# sourceMappingURL=PromiseControls.js.map