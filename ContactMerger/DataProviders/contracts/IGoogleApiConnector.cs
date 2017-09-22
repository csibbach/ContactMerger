using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.People.v1.Data;

namespace ContactMerger.DataProviders.contracts
{
    public interface IGoogleApiConnector
    {
        Task<string> GetEmailAddressForCredential(UserCredential credential);
        Task<IList<Person>> GetContacts(UserCredential credential);
        Task<string> AddContact(UserCredential credential, string firstName, string lastName, string email);
    }
}
