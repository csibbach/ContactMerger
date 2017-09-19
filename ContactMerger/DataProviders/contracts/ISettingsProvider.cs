namespace ContactMerger.DataProviders.contracts
{
    public interface ISettingsProvider
    {
        string GetGoogleClientId();
        string GetGoogleClientSecret();
    }
}
