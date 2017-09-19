using Google.Apis.Auth.OAuth2;
using Google.Apis.People.v1;

namespace ContactMerger.Factories.contracts
{
    public interface IGoogleServiceFactory
    {
        PeopleService CreatePeopleService(UserCredential credential);
    }
}
