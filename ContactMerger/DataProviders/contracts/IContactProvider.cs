using System.Threading.Tasks;
using ContactMerger.Models;

namespace ContactMerger.DataProviders.contracts
{
    public interface IContactProvider
    {
        Task<ContactList> GetContacts(string username, string accountEmail);
    }
}
