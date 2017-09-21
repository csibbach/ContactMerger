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

            // Loop through each list
            foreach (var contactList in contactLists)
            {
                // For quicker reference
                var email = contactList.Account.AccountEmail;

                // We will either add a ContactRelationship to every column, or update the info
                // in the existing columns.
                foreach (var contact in contactList.Contacts)
                {
                    // Check if there is an existing relationship anywhere in the grid.
                    var existingIndex = await FindExistingRelationship(contact, returnSet.ContactGrid);
                    if (existingIndex >= 0)
                    {
                        // Update this one based on the contact, and update the status variables
                        UpdateContactRow(contact, email, existingIndex, returnSet.ContactGrid);
                    }
                    else
                    {
                        // No existing row anywhere for this contact, add a new set.
                        AddContactRow(contact, email, returnSet.ContactGrid);
                    }
                }
            }

            // Last thing to do is order the list, in our case by the email
            //returnSet.Relationships = returnSet.Relationships.OrderBy(x => x.Contact.Email).ToList();

            return returnSet;
        }

        public Task<bool> ContactsMatch(Contact contact1, Contact contact2)
        {
            // Check the params
            if (contact1 == null) throw new ArgumentNullException(nameof(contact1));
            if (contact2 == null) throw new ArgumentNullException(nameof(contact2));

            // Prep the data. Could do a lot of regex stuff, look for nicknames, etc.
            var email1 = contact1.Email.Trim().ToLowerInvariant();
            var email2 = contact2.Email.Trim().ToLowerInvariant();
            var firstName1 = contact1.FirstName.Trim().ToLowerInvariant();
            var firstName2 = contact2.FirstName.Trim().ToLowerInvariant();
            var lastName1 = contact1.LastName.Trim().ToLowerInvariant();
            var lastName2 = contact2.LastName.Trim().ToLowerInvariant();

            // I'm going to say they match if EITHER the email matches or the first and last names 
            // match.
            return Task.FromResult(email1 == email2  || firstName1 == firstName2 && lastName1 == lastName2);
        }

        protected void UpdateContactRow(Contact contact, string accountEmail,
            int existingIndex, IDictionary<string, IList<ContactRelationship>> contactGrid)
        {
            // First, update the existing contact relationship with info from the contact
            var updateRelationship =
                contactGrid[accountEmail][existingIndex];
            updateRelationship.ContactExists = true;
            updateRelationship.FirstName = contact.FirstName;
            updateRelationship.LastName = contact.LastName;
            updateRelationship.Email = contact.Email;

            // Now we update some of the status markers- what fields do not match
            // We have to loop through all the contacts in the row, and update their
            // status markers as well if we find a field that doesn't match
            foreach (var column in contactGrid)
            {
                // If we're on the column this contact belongs to get out of here.
                if (column.Key == accountEmail) continue;

                // Not the column we're working on, an existing column
                var existingRelationship = column.Value[existingIndex];

                // If the ContactRelationship does not exist we can get out of here.
                if (!existingRelationship.ContactExists) continue;

                // Check each bit of info
                if (existingRelationship.FirstName != updateRelationship.FirstName)
                {
                    existingRelationship.FirstNameMatches = false;
                    updateRelationship.FirstNameMatches = false;
                }
                if (existingRelationship.LastName != updateRelationship.LastName)
                {
                    existingRelationship.LastNameMatches = false;
                    updateRelationship.LastNameMatches = false;
                }
                if (existingRelationship.Email != updateRelationship.Email)
                {
                    existingRelationship.EmailMatches = false;
                    updateRelationship.EmailMatches = false;
                }
            }
        }

        protected void AddContactRow(Contact contact, string accountEmail,
            IDictionary<string, IList<ContactRelationship>> contactGrid)
        {
            // Create a new ContactRelationship in each column
            foreach (var column in contactGrid)
            {
                if (column.Key == accountEmail)
                {
                    column.Value.Add(_contactFactory.CreateContactRelationship(contact));
                }
                else
                {
                    column.Value.Add(_contactFactory.CreateEmptyContactRelationship());
                }
            }
        }

        /// <summary>
        /// Helper function to find the existing relationship if it exists. Returns null if
        /// one does not exist. Returns the index of the existing column or -1 if not found
        /// </summary>
        /// <param name="contact"></param>
        /// <param name="contactGrid"></param>
        /// <returns></returns>
        protected async Task<int> FindExistingRelationship(Contact contact, 
            IDictionary<string, IList<ContactRelationship>> contactGrid)
        {
            foreach (var column in contactGrid.Values)
            {
                // I need the index in the column; it will be the same for all the columns
                for (var i = 0; i < column.Count; i++)
                {
                    var contactRelationship = column[i];
                    if (await ContactsMatch(contact, contactRelationship))
                    {
                        return i;
                    }
                }
            }

            // No relationship found
            return -1;
        }
    }
}