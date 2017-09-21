import PromiseControls = require("tests/utility/PromiseControls");
import IAjaxConnector = require("dataProviders/contracts/IAjaxConnector");

class AjaxConnectorMock implements IAjaxConnector {
    public constructor(private assert: QUnitAssert) { }

    public getPromise = new PromiseControls<any>(this.assert);
    public get<T>(url: string): Promise<T> {
        return this.getPromise.promise;
    }
};

export = AjaxConnectorMock;
