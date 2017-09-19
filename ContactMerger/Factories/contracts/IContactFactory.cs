using ContactMerger.Models;

namespace ContactMerger.Factories.contracts
{
    public interface IContactFactory
    {
        Contact CreateContact(string firstName, string lastName, string email);

        ContactList CreateContactList(string accountEmail, EContactAccountType contactAccountType);
    }
}
