define(["require", "exports", "tests/utility/PromiseControls"], function (require, exports, PromiseControls) {
    "use strict";
    var AjaxConnectorMock = (function () {
        function AjaxConnectorMock(assert) {
            this.assert = assert;
            this.getPromise = new PromiseControls(this.assert);
            this.postPromise = new PromiseControls(this.assert);
        }
        AjaxConnectorMock.prototype.get = function (url) {
            return this.getPromise.promise;
        };
        AjaxConnectorMock.prototype.post = function (url, content) {
            return this.postPromise.promise;
        };
        return AjaxConnectorMock;
    }());
    ;
    return AjaxConnectorMock;
});
//# sourceMappingURL=AjaxConnectorMock.js.map