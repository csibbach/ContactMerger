import PromiseControls = require("tests/utility/PromiseControls");
import IAjaxConnector = require("dataProviders/contracts/IAjaxConnector");

class AjaxConnectorMock implements IAjaxConnector {
    public constructor(private assert: QUnitAssert) { }

    public getUrl: string;
    public getControls = new PromiseControls<any>(this.assert);
    public get<T>(url: string): Promise<T> {
        this.getUrl = url;
        return this.getControls.promise;
    }

    public postUrl: string;
    public postContent: any;
    public postControls = new PromiseControls<any>(this.assert);
    public post<T>(url: string, content: any): Promise<T> {
        this.postUrl = url;
        this.postContent = content;
        return this.postControls.promise;
    }
};

export = AjaxConnectorMock;
