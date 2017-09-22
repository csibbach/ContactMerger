using System.Collections.Generic;
using System.Threading.Tasks;
using ContactMerger.DataProviders.contracts;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Moq;
using Google.Apis.People.v1.Data;

namespace ContactMerger.Tests.Mocks
{
    public static class MyMockFactory
    {
        public const string Username = "username";
        public const string CredentialEmail1 = "account1@google.com";
        public const string CredentialEmail2 = "account2@google.com";
        public const string BadCredentialEmail = "bad@nonexistent.xxx";
        public static readonly UserCredential UserCredential1;
        public static readonly UserCredential UserCredential2;

        // Yeah yeah, all the arguments about static constructors. This is unit test code not some multithread nightmare.
        static MyMockFactory()
        {
            var authorizationCodeFlow = new Mock<IAuthorizationCodeFlow>();
            UserCredential1 = new UserCredential(authorizationCodeFlow.Object, "account1", new TokenResponse());
            UserCredential2 = new UserCredential(authorizationCodeFlow.Object, "account2", new TokenResponse());
        }

        public static Mock<IGoogleCredentialProvider> CreateGoogleCredentialProviderMock()
        {
            var credentialProviderMock = new Mock<IGoogleCredentialProvider>();

            // Stored credentials
            var storedCredentials = new Dictionary<string, UserCredential>
            {
                {CredentialEmail1, UserCredential1},
                {CredentialEmail2, UserCredential2}
            };
            credentialProviderMock.Setup(x => x.GetCredentials(Username))
                .Returns(Task.FromResult((IDictionary<string, UserCredential>) storedCredentials));

            return credentialProviderMock;
        }

        public static Mock<IGoogleApiConnector> CreateGoogleApiConnectorMock(
            IList<Person> peopleForGetContactsCredential1 = null,
            IList<Person> peopleForGetContactsCredential2 = null)
        {
            var connectorMock = new Mock<IGoogleApiConnector>();
            connectorMock.Setup(x => x.GetContacts(UserCredential1))
                .ReturnsAsync(peopleForGetContactsCredential1 ?? new List<Person>());
            connectorMock.Setup(x => x.GetContacts(UserCredential2))
                .ReturnsAsync(peopleForGetContactsCredential2 ?? new List<Person>());

            connectorMock.Setup(x => x.GetEmailAddressForCredential(UserCredential1)).ReturnsAsync(CredentialEmail1);
            connectorMock.Setup(x => x.GetEmailAddressForCredential(UserCredential2)).ReturnsAsync(CredentialEmail2);

            connectorMock.Setup(x =>
                    x.AddContact(It.IsAny<UserCredential>(), It.IsAny<string>(), It.IsAny<string>(),
                        It.IsAny<string>()))
                .ReturnsAsync("junk string");

            return connectorMock;
        }

        public static Person CreatePerson(string firstName = null, string lastName = null, string email = null)
        {
            var person = new Person();

            if (firstName != null || lastName != null)
            {
                person.Names = new List<Name>
                {
                    new Name
                    {
                        GivenName = firstName,
                        FamilyName = lastName
                    }
                };
            }

            if (email != null)
            {
                person.EmailAddresses = new List<EmailAddress>
                {
                    new EmailAddress
                    {
                        Value = email
                    }
                };
            }

            return person;
        }
    }
}
