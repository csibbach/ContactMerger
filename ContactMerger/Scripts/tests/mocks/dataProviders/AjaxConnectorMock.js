define(["require", "exports", "tests/utility/PromiseControls"], function (require, exports, PromiseControls) {
    "use strict";
    var AjaxConnectorMock = (function () {
        function AjaxConnectorMock(assert) {
            this.assert = assert;
            this.getControls = new PromiseControls(this.assert);
            this.postControls = new PromiseControls(this.assert);
        }
        AjaxConnectorMock.prototype.get = function (url) {
            this.getUrl = url;
            return this.getControls.promise;
        };
        AjaxConnectorMock.prototype.post = function (url, content) {
            this.postUrl = url;
            this.postContent = content;
            return this.postControls.promise;
        };
        return AjaxConnectorMock;
    }());
    ;
    return AjaxConnectorMock;
});
//# sourceMappingURL=AjaxConnectorMock.js.map