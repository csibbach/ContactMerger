using System.Collections.Generic;
using ContactMerger.DataProviders.contracts;
using Google.Apis.Auth.OAuth2;

namespace ContactMerger.DataProviders.implementations
{
    public class GoogleCredentialProvider: IGoogleCredentialProvider
    {
        private readonly Dictionary<string, List<UserCredential>> _credentials;

        public GoogleCredentialProvider()
        {
            _credentials = new Dictionary<string, List<UserCredential>>();   
        }

        public void SaveCredential(string username, UserCredential credential)
        {
            // Get the current list of credentials for this username. Create the list if it doesn't
            // exist.
            if (_credentials.ContainsKey(username))
            {
                var userCreds = _credentials[username];
                
                userCreds.Add(credential);
            }
            else
            {
                _credentials.Add(username, new List<UserCredential>());
            }
        }
     
        public IEnumerable<UserCredential> GetCredentials(string username)
        {
            // Make sure to return a list, even an empty one. Just makes life easier!
            // Get the current list of credentials for this username. Create the list if it doesn't
            // exist.
            var userCreds = _credentials[username];
            if (userCreds == null)
            {
                userCreds = new List<UserCredential>();
                _credentials.Add(username, userCreds);
            }
            return userCreds;
        }
    }
}