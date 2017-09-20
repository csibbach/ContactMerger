using System.Collections.Generic;
using ContactMerger.Factories.contracts;
using ContactMerger.Models;

namespace ContactMerger.Factories.implementations
{
    public class ContactFactory: IContactFactory
    {
        public Contact CreateContact(string firstName, string lastName, string email)
        {
            // We should do some parameter checking here.
            return new Contact
            {
                Email = email,
                FirstName = firstName,
                LastName = lastName
            };
        }

        public ContactAccount CreateContactAccount(string accountEmail, EContactAccountType contactAccountType)
        {
            return new ContactAccount
            {
                AccountEmail = accountEmail,
                ContactAccountType = contactAccountType
            };
        }


        public ContactList CreateContactList(string accountEmail, EContactAccountType contactAccountType)
        {
            return new ContactList
            {
                Account = CreateContactAccount(accountEmail, contactAccountType),
                Contacts = new List<Contact>()
            };
        }

        public ContactSet CreateContactSet(IList<ContactAccount> accounts)
        {
            return new ContactSet
            {
                Accounts = accounts,
                Relationships = new List<ContactRelationship>()
            };
        }

        public ContactRelationship CreateContactRelationship(Contact contact, string accountEmail)
        {
            return new ContactRelationship
            {
                Contact = contact,
                ContactAccountMap = new List<string> { accountEmail}
            };
        }
    }
}