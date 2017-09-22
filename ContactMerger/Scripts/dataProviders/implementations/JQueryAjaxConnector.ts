import IAjaxConnector = require("dataProviders/contracts/IAjaxConnector");
import $ = require("jquery");

// JQuery based implementation of IAjaxConnector
class JQueryAjaxConnector implements IAjaxConnector {
    public get<T>(url: string): Promise<T> {
        return Promise.resolve($.get(url));
    }

    public post<T>(url: string, content: any): Promise<T> {
        return Promise.resolve($.post(url, content));
    }
}

export = JQueryAjaxConnector;