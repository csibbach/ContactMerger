import ko = require("knockout");

class Contact {
    public firstName: string;
    public lastName: string;
    public email: string;
    public firstNameMatches: boolean;
    public lastNameMatches: boolean;
    public emailMatches: boolean;
    public contactExists: boolean;
    public accountEmail: string;
}

export = Contact;