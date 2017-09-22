// Wrapper class abstraction, usually around JQuery. Very useful for testing, as you can now
// easily mock API calls.
interface IAjaxConnector
{
    get<T>(url: string): Promise<T>;
    post<T>(url: string, content: any): Promise<T>;
}

export = IAjaxConnector;