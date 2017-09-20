using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;

namespace ContactMerger.DataProviders.contracts
{
    public interface IGoogleCredentialProvider
    {
        // SaveCredential saves the credential into memory, and returns the email address
        // associated with it.
        Task<string> SaveCredential(string username, UserCredential credential);
        IDictionary<string, UserCredential> GetCredentials(string username);
    }
}
