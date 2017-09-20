interface IContactAccountConnector {
    getContactAccounts(): Promise<string[]>;
};

export = IContactAccountConnector;