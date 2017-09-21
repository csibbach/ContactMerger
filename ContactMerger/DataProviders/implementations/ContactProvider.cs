using System.Collections.Generic;
using System.Threading.Tasks;
using ContactMerger.DataProviders.contracts;
using ContactMerger.Factories.contracts;
using ContactMerger.Models;
using Google.Apis.Auth.OAuth2;
using Google.Apis.People.v1;
using Google.Apis.People.v1.Data;

namespace ContactMerger.DataProviders.implementations
{
    // This implementation of ContactProvider only 
    public class ContactProvider : IContactProvider
    {
        private readonly IGoogleCredentialProvider _googleCredentialProvider;
        private readonly IContactFactory _contactFactory;
        private readonly IGoogleServiceFactory _googleServiceFactory;

        public ContactProvider(IGoogleCredentialProvider googleCredentialProvider,
            IContactFactory contactFactory,
            IGoogleServiceFactory googleServiceFactory)
        {
            // Should do null checks here.
            _googleCredentialProvider = googleCredentialProvider;
            _contactFactory = contactFactory;
            _googleServiceFactory = googleServiceFactory;
        }

        public async Task<ContactList> GetContacts(string username, string accountEmail)
        {
            // Create the return list. Saves doing it in several places
            var returnList = _contactFactory.CreateContactList(accountEmail, EContactAccountType.Google);

            // Get the credential for that email. If it's not registered already we just return
            // an empty ContactList.
            var userCredentials = await _googleCredentialProvider.GetCredentials(username);

            // Get the stored credential
            if (!userCredentials.ContainsKey(accountEmail))
            {
                // If there is no user credential, bail out here, return an empty list
                return returnList;
            }
            UserCredential userCredential = userCredentials[accountEmail];

            // Create a Google service object
            var peopleService = _googleServiceFactory.CreatePeopleService(userCredential);

            PeopleResource.ConnectionsResource.ListRequest peopleRequest =
                peopleService.People.Connections.List("people/me");
            peopleRequest.RequestMaskIncludeField = "person.names,person.emailAddresses";
            var connectionsResponse = await peopleRequest.ExecuteAsync();
            IList<Person> connections = connectionsResponse.Connections;

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
    }
}