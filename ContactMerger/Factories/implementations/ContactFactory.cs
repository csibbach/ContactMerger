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
                email = email,
                firstName = firstName,
                lastName = lastName
            };
        }

        public ContactList CreateContactList(string accountEmail, EContactAccountType contactAccountType)
        {
            return new ContactList
            {
                AccountEmail = accountEmail,
                ContactAccountType = contactAccountType
            };
        }
    }
}