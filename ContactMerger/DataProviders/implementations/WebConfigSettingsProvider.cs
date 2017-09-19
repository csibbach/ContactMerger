using System;
using System.Configuration;
using System.Globalization;
using ContactMerger.DataProviders.contracts;

namespace ContactMerger.DataProviders.implementations
{
    /// <summary>
    /// This implementation of ISettingsProvider gets the settings from the web.config
    /// </summary>
    public class WebConfigSettingsProvider: ISettingsProvider
    {
        private static T Setting<T>(string name)
        {
            string value = ConfigurationManager.AppSettings[name];

            if (value == null)
            {
                throw new Exception($"Could not find setting '{name}',");
            }

            return (T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture);
        }

        public string GetGoogleClientId()
        {
            return Setting<string>("GoogClientID");
        }

        public string GetGoogleClientSecret()
        {
            return Setting<string>("GoogClientSecret");
        }
    }
}