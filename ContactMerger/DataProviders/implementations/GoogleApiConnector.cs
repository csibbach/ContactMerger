using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ContactMerger.DataProviders.contracts;
using Google.Apis.Auth.OAuth2;
using Google.Apis.People.v1;
using Google.Apis.People.v1.Data;
using Google.Apis.Services;

namespace ContactMerger.DataProviders.implementations
{
    /// <summary>
    /// This class is untestable without fakes. I've put all the external non-mockable stuff here.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class GoogleApiConnector: IGoogleApiConnector
    {
        private readonly HttpClient _httpClient = new HttpClient();

        public async Task<string> GetEmailAddressForCredential(UserCredential credential)
        {
            // Create the service object
            var service =  new PeopleService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = "ContactMerger"
            });

            var peopleRequest = service.People.Get("people/me");
            peopleRequest.RequestMaskIncludeField = "person.names,person.emailAddresses";
            var connectionsResponse = await peopleRequest.ExecuteAsync();

            // Get the email address from the response.
            // TODO: Error handling; if they don't have an email address this will break
            return connectionsResponse.EmailAddresses.First().Value;
        }

        public async Task<IList<Person>> GetContacts(UserCredential credential)
        {
            // Create the service object
            var service = new PeopleService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = "ContactMerger"
            });

            PeopleResource.ConnectionsResource.ListRequest peopleRequest =
                service.People.Connections.List("person/me");
            peopleRequest.RequestMaskIncludeField = "person.names,person.emailAddresses";
            var connectionsResponse = await peopleRequest.ExecuteAsync();
            return connectionsResponse.Connections;
        }

        public async Task<string> AddContact(UserCredential credential, string firstName, string lastName, string email)
        {
            // Oh boy does this ever suck. The google api nuget package does not include the calls I need,
            // even though they are in the documentation, and that is the latest distribution of it, even
            // on google's site. I have to do this via post requests directly. 
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, "https://people.googleapis.com/v1/people:createContact?fields=emailAddresses%2Cnames");
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", credential.Token.AccessToken);
            var requestContent = $"{{\"names\": [{{\"familyName\": \"{lastName}\",\"givenName\": \"{firstName}\"}}],\"emailAddresses\": [{{\"value\":\"{email}\"}}]}}";
            requestMessage.Content = new StringContent(requestContent, Encoding.UTF8, "application/json");

            // Send the request
            var response = await _httpClient.SendAsync(requestMessage);

            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            return responseString;
        }
    }
}