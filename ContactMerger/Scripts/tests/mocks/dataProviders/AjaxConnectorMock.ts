import PromiseControls = require("tests/utility/PromiseControls");
import IAjaxConnector = require("dataProviders/contracts/IAjaxConnector");

class AjaxConnectorMock implements IAjaxConnector {
    public constructor(private assert: QUnitAssert) { }

    public getPromise = new PromiseControls<any>(this.assert);
    public get<T>(url: string): Promise<T> {
        return this.getPromise.promise;
    }

    public postPromise = new PromiseControls<any>(this.assert);
    public post<T>(url: string, content: any): Promise<T> {
        return this.postPromise.promise;
    }
};

export = AjaxConnectorMock;
