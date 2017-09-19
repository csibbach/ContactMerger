using System.Threading.Tasks;
using ContactMerger.Models;

namespace ContactMerger.DataProviders.contracts
{
    interface IContactProvider
    {
        Task<ContactList> GetContacts(string username, string accountEmail);
    }
}
