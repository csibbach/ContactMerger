using System;
using System.Threading.Tasks;
using ContactMerger.DataProviders.contracts;
using ContactMerger.Factories.contracts;
using ContactMerger.Models;
using Google.Apis.Auth.OAuth2;

namespace ContactMerger.DataProviders.implementations
{
    // This implementation of ContactProvider only 
    public class ContactProvider : IContactProvider
    {
        private readonly IGoogleCredentialProvider _googleCredentialProvider;
        private readonly IContactFactory _contactFactory;
        private readonly IGoogleApiConnector _googleApiConnector;

        public ContactProvider(IGoogleCredentialProvider googleCredentialProvider,
            IContactFactory contactFactory,
            IGoogleApiConnector googleApiConnector)
        {
            // Should do null checks here.
            _googleCredentialProvider = googleCredentialProvider;
            _contactFactory = contactFactory;
            _googleApiConnector = googleApiConnector;
        }

        public async Task<ContactList> GetContacts(string username, string accountEmail)
        {
            // Create the return list. Saves doing it in several places
            var returnList = _contactFactory.CreateContactList(accountEmail, EContactAccountType.Google);

            // Get the credential for that email. If it's not registered already we just return
            // an empty ContactList.
            var userCredential = await GetCredential(username, accountEmail);
            if (userCredential == null)
            {
                // If there is no user credential, bail out here, return an empty list
                return returnList;
            }

            // Get the connections
            var connections = await _googleApiConnector.GetContacts(userCredential);

            // Map all the persons into Contacts
            foreach (var connection in connections)
            {
                // Google contacts may not have any of the critical info we need, and any of the fields
                // may be null, so we need to protect against that
                // Honestly I'm not 100% sold on this code here. I know there is a better way
                // to do all these null checks but it's late and I can't figure it out right now.
                var firstName = connection.Names != null && connection.Names.Count > 0 ? connection.Names[0].GivenName : "";
                var lastName = connection.Names != null && connection.Names.Count > 0 ? connection.Names[0].FamilyName : "";
                var emailAddress = connection.EmailAddresses != null && connection.EmailAddresses.Count > 0 ? connection.EmailAddresses[0].Value : "";  

                // firstName and lastName may still be null if the contact only has one or the other.
                firstName = firstName ?? "";
                lastName = lastName ?? "";
                emailAddress = emailAddress ?? ""; // Just for good measure.

                returnList.Contacts.Add(_contactFactory.CreateContact(firstName,
                    lastName, emailAddress
                ));
            }

            return returnList;
        }


        public async Task<string> AddContact(string username, string accountEmail, string firstName, string lastName, string email)
        {
            // Get the credential. 
            var userCredential = await GetCredential(username, accountEmail);
            if (userCredential == null)
            {
                throw new ArgumentException("No credential for requested account!");
            }

            return await _googleApiConnector.AddContact(userCredential, firstName, lastName, email);
        }

        private async Task<UserCredential> GetCredential(string username, string accountEmail)
        {
            // Get the credential for that email. If it's not registered already we just return
            // an empty ContactList.
            var userCredentials = await _googleCredentialProvider.GetCredentials(username);

            // Get the stored credential
            if (!userCredentials.ContainsKey(accountEmail))
            {
                // If there is no user credential, bail out here, return null
                return null;
            }
            return userCredentials[accountEmail];
        }
    }
}