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
            var set = new ContactSet
            {
                ContactGrid = new Dictionary<string, IList<ContactRelationship>>()
            };

            // Create a column in the grid for each account
            foreach (var account in accounts)
            {
                set.ContactGrid.Add(account.AccountEmail, new List<ContactRelationship>());
            }

            return set;
        }

        public ContactRelationship CreateContactRelationship(Contact contact)
        {
            return new ContactRelationship
            {
                FirstName = contact.FirstName,
                LastName = contact.LastName,
                Email =  contact.Email,
                FirstNameMatches = true,
                LastNameMatches = true,
                EmailMatches = true,
                ContactExists = true
            };
        }

        public ContactRelationship CreateEmptyContactRelationship()
        {
            return new ContactRelationship
            {
                FirstName = "",
                LastName = "",
                Email = "",
                FirstNameMatches = true,
                LastNameMatches = true,
                EmailMatches = true,
                ContactExists = false
            };
        }
    }
}