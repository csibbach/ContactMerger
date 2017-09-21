import IAjaxConnector = require("dataProviders/contracts/IAjaxConnector");
import $ = require("jquery");

// JQuery based implementation of IAjaxConnector
class JQueryAjaxConnector implements IAjaxConnector {
    public get<T>(url: string): Promise<T> {
        return Promise.resolve($.get(url));
    }
}

export = JQueryAjaxConnector;