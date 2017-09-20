define(["require", "exports"], function (require, exports) {
    "use strict";
    var PromiseUtils = (function () {
        function PromiseUtils() {
        }
        PromiseUtils.afterResolved = function (assert, promise, thenFunction) {
            var done = assert.async();
            promise.then(function (thenParams) {
                thenFunction(thenParams);
                done();
            });
        };
        PromiseUtils.assertPending = function (promise, pendingFunction, assert, assertMessage, pendingTime) {
            if (assertMessage === void 0) { assertMessage = null; }
            if (pendingTime === void 0) { pendingTime = 100; }
            var done = assert.async();
            var failed = false;
            promise.then(function () {
                assert.equal("resolved", "pending", assertMessage);
                failed = true;
                done();
            }, function () {
                failed = true;
                assert.equal("rejected", "pending", assertMessage);
                done();
            });
            setTimeout(function () {
                if (!failed) {
                    assert.equal("pending", "pending", assertMessage);
                    pendingFunction();
                    done();
                }
            }, pendingTime);
        };
        PromiseUtils.assertResolved = function (promise, thenFunction, assert, assertMessage, pendingTime) {
            if (assertMessage === void 0) { assertMessage = null; }
            if (pendingTime === void 0) { pendingTime = 100; }
            var done = assert.async();
            var handled = false;
            promise.then(function (thenParams) {
                handled = true;
                assert.equal("resolved", "resolved", assertMessage);
                try {
                    thenFunction(thenParams);
                }
                catch (error) {
                    assert.equal("then function runs", "exception in then function", "Then callback for assertResolved has an error!!. \n" + error);
                }
                done();
            }, function () {
                handled = true;
                assert.equal("rejected", "resolved", assertMessage);
                done();
            });
            setTimeout(function () {
                if (!handled) {
                    assert.equal("pending", "resolved", assertMessage);
                    done();
                }
            }, pendingTime);
        };
        PromiseUtils.assertRejected = function (promise, catchFunction, assert, assertMessage, pendingTime) {
            if (assertMessage === void 0) { assertMessage = null; }
            if (pendingTime === void 0) { pendingTime = 100; }
            var done = assert.async();
            var handled = false;
            promise.then(function () {
                handled = true;
                assert.equal("resolved", "rejected", assertMessage);
                done();
            }, function (catchError) {
                handled = true;
                assert.equal("rejected", "rejected", assertMessage);
                catchFunction(catchError);
                done();
            });
            setTimeout(function () {
                if (!handled) {
                    assert.equal("pending", "rejected", assertMessage);
                    done();
                }
            }, pendingTime);
        };
        PromiseUtils.asyncAction = function (assert) {
            var firstAction = new Action(assert);
            firstAction.activate();
            return firstAction;
        };
        return PromiseUtils;
    }());
    var Action = (function () {
        function Action(assert) {
            this.assert = assert;
        }
        Action.prototype.act = function (callback) {
            this.done = this.assert.async();
            this.next = new Action(this.assert);
            this.callback = callback;
            return this.next;
        };
        Action.prototype.activate = function () {
            var _this = this;
            setTimeout(function () {
                if (_this.callback != null) {
                    _this.callback();
                    _this.next.activate();
                    _this.done();
                }
            }, 1);
        };
        return Action;
    }());
    return PromiseUtils;
});
//# sourceMappingURL=PromiseUtils.js.map