using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContactMerger.DataProviders.contracts;
using ContactMerger.Factories.contracts;
using Google.Apis.Auth.OAuth2;

namespace ContactMerger.DataProviders.implementations
{
    public class GoogleCredentialProvider: IGoogleCredentialProvider
    {
        private readonly IGoogleServiceFactory _googleServiceFactory;
        private readonly Dictionary<string, Dictionary<string, UserCredential>> _credentials;

        public GoogleCredentialProvider(IGoogleServiceFactory googleServiceFactory)
        {
            _googleServiceFactory = googleServiceFactory;
            _credentials = new Dictionary<string, Dictionary<string, UserCredential>>();
        }

        public async Task<string> SaveCredential(string username, UserCredential credential)
        {
            // We have to turn the credential into an email address for storage.
            // It would be really cool if this info was given to us via the credential itself
            // but such is not the case.
            // Create a Google service object
            var peopleService = _googleServiceFactory.CreatePeopleService(credential);

            var peopleRequest =
                peopleService.People.Get("people/me");
            peopleRequest.RequestMaskIncludeField = "person.names,person.emailAddresses";
            var connectionsResponse = await peopleRequest.ExecuteAsync();

            // Get the email address from the response.
            // TODO: Error handling; if they don't have an email address this will break
            var email = connectionsResponse.EmailAddresses.First().Value;

            // Get the current list of credentials for this username. Create the list if it doesn't
            // exist.
            if (_credentials.ContainsKey(username))
            {
                // This user already has some credentials. Check if they have this account
                // in particular authorized.
                var userCreds = _credentials[username];

                // Check if the user already has a credential for this account Id
                if (userCreds.ContainsKey(email))
                {
                    // Already there, replace it with the new credential!
                    userCreds[email] = credential;
                }
                else
                {
                    // Not already there, add it
                    userCreds.Add(email, credential);
                }
            }
            else
            {
                // No entries for this user at all.
                var newCredentialList = new Dictionary<string, UserCredential>
                {
                    {email, credential}
                };
                _credentials.Add(username, newCredentialList);
            }

            return email;
        }
     
        public Task<IDictionary<string, UserCredential>> GetCredentials(string username)
        {
            // Make sure to return a list, even an empty one. Just makes life easier!
            // Get the current list of credentials for this username. Create the list if it doesn't
            // exist.
            if (_credentials.ContainsKey(username))
            {
                var userCreds = _credentials[username];

                // Return just a list of stored credentials without the dictionary keying.
                return Task.FromResult((IDictionary<string, UserCredential>) userCreds);
            }

            // Does not already have credentials
            _credentials.Add(username, new Dictionary<string, UserCredential>());
            return Task.FromResult(_credentials[username] as IDictionary<string, UserCredential>);
        }
    }
}