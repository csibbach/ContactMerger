using ContactMerger.Factories.contracts;
using Google.Apis.Auth.OAuth2;
using Google.Apis.People.v1;
using Google.Apis.Services;

namespace ContactMerger.Factories.implementations
{
    public class GoogleServiceFactory: IGoogleServiceFactory
    {
        public PeopleService CreatePeopleService(UserCredential userCredential)
        {
            return new PeopleService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = userCredential,
                ApplicationName = "ContactMerger",
            });
        }
    }
}