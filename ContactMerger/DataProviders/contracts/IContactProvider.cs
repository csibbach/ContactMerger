using System.Threading.Tasks;
using ContactMerger.Models;

namespace ContactMerger.DataProviders.contracts
{
    public interface IContactProvider
    {
        Task<ContactList> GetContacts(string username, string accountEmail);

        Task<string> AddContact(string username, string accountEmail, string firstName, string lastName, string email);
    }
}
