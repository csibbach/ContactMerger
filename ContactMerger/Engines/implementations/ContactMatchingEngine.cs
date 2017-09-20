using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContactMerger.Engines.contracts;
using ContactMerger.Factories.contracts;
using ContactMerger.Models;

namespace ContactMerger.Engines.implementations
{
    public class ContactMatchingEngine: IContactMatchingEngine
    {
        private readonly IContactFactory _contactFactory;

        public ContactMatchingEngine(IContactFactory contactFactory)
        {
            _contactFactory = contactFactory;
        }

        public async Task<ContactSet> MergeContactLists(IList<ContactList> contactLists)
        {
            // Check params
            if (contactLists == null)
            {
                throw new ArgumentNullException(nameof(contactLists));
            }

            // Create the return ContactSet
            var accounts = contactLists.Select(x => x.Account).ToList();
            var returnSet = _contactFactory.CreateContactSet(accounts);

            // Loop through each list, and add the contacts as a relationship, or update
            // the relationship if it already exists.
            foreach (var contactList in contactLists)
            {
                foreach (var contact in contactList.Contacts)
                {
                    // Check for an existing relationship
                    var existingRelationship = await FindExistingRelationship(contact, returnSet.Relationships);
                    if (existingRelationship != null)
                    {
                        // One exists, add a reference to the account
                        existingRelationship.ContactAccountMap.Add(contactList.Account.AccountEmail);
                    }
                    else
                    {
                        // Need to create a relationship
                        returnSet.Relationships.Add(_contactFactory.CreateContactRelationship(contact, 
                            contactList.Account.AccountEmail));
                    }
                }
            }

            // Last thing to do is order the list, in our case by the email
            returnSet.Relationships = returnSet.Relationships.OrderBy(x => x.Contact.Email).ToList();

            return returnSet;
        }

        public Task<bool> ContactsMatch(Contact contact1, Contact contact2)
        {
            // Check the params
            if (contact1 == null) throw new ArgumentNullException(nameof(contact1));
            if (contact2 == null) throw new ArgumentNullException(nameof(contact2));

            // Prep the data. Could do a lot of regex stuff, look for nicknames, etc.
            // I'm just going to compare email, first and last name.
            // Coincidentally the only info I'm tracking in a contact...
            var email1 = contact1.Email.Trim().ToLowerInvariant();
            var email2 = contact2.Email.Trim().ToLowerInvariant();
            var firstName1 = contact1.FirstName.Trim().ToLowerInvariant();
            var firstName2 = contact2.FirstName.Trim().ToLowerInvariant();
            var lastName1 = contact1.LastName.Trim().ToLowerInvariant();
            var lastName2 = contact2.LastName.Trim().ToLowerInvariant();

            return Task.FromResult(email1 == email2 && firstName1 == firstName2 && lastName1 == lastName2);
        }

        // Helper function to find the existing relationship if it exists. Returns null if
        // one does not exist.
        protected async Task<ContactRelationship> FindExistingRelationship(Contact contact, IList<ContactRelationship> relationships)
        {
            foreach (var relationship in relationships)
            {
                if (await ContactsMatch(contact, relationship.Contact))
                {
                    return relationship;
                }
            }

            // No relationship found
            return null;
        }
    }
}