using System.Collections.Generic;
using Google.Apis.Auth.OAuth2;

namespace ContactMerger.DataProviders.contracts
{
    public interface IGoogleCredentialProvider
    {
        void SaveCredential(string username, UserCredential credential);
        IEnumerable<UserCredential> GetCredentials(string username);
    }
}
