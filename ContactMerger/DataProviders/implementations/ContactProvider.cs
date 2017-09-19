using System.Collections.Generic;
using System.Linq;
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
            var userCredentials = _googleCredentialProvider.GetCredentials(username);

            // Get the stored credential
            // Note, I use LINQ along with foreach and other methods, depends on what is easier
            // to read. Usually loops that are 1 deep are LINQ, more than that I write
            UserCredential userCredential = userCredentials.FirstOrDefault(tempCredential => tempCredential.UserId == accountEmail);

            // If there is no user credential, bail out here, return an empty list
            if (userCredential == null)
            {
                return returnList;
            }

            // Create a Google service object
            var peopleService = _googleServiceFactory.CreatePeopleService(userCredential);

            PeopleResource.ConnectionsResource.ListRequest peopleRequest =
                peopleService.People.Connections.List("people/me");
            //peopleRequest.PersonFields = "names,emailAddresses";
            var connectionsResponse = await peopleRequest.ExecuteAsync();
            IList<Person> connections = connectionsResponse.Connections;

            // Map all the persons into Contacts
            foreach (var connection in connections)
            {
                returnList.Contacts.Add(_contactFactory.CreateContact(connection.Names[0].GivenName,
                    connection.Names[0].FamilyName,
                    connection.EmailAddresses[0].Value
                ));
            }

            return returnList;
        }
    }
}