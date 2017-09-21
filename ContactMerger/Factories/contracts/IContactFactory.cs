using System.Collections.Generic;
using ContactMerger.Models;

namespace ContactMerger.Factories.contracts
{
    public interface IContactFactory
    {
        Contact CreateContact(string firstName, string lastName, string email);

        ContactAccount CreateContactAccount(string accountEmail, EContactAccountType contactAccountType);

        ContactList CreateContactList(string accountEmail, EContactAccountType contactAccountType);

        ContactSet CreateContactSet(IList<ContactAccount> accounts);

        ContactRelationship CreateContactRelationship(Contact contact, string accountEmail);

        ContactRelationship CreateEmptyContactRelationship(string accountEmail);
    }
}
