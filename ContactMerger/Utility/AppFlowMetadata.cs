using System;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Web.Mvc;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Mvc;
using Google.Apis.People.v1;
using Google.Apis.Util.Store;

namespace ContactMerger.Utility
{
    /// <inheritdoc />
    /// <summary>
    /// Excluding from coverage as this is just example code from Google, altered only to use the web.config settings
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class AppFlowMetadata : FlowMetadata
    {
        private readonly string _username;

        private static IAuthorizationCodeFlow _flowInstance;

        public AppFlowMetadata(string username)
        {
            _username = username;
        }

        public override string GetUserId(Controller controller)
        {
            // AH HA. Found the issue. Basically, google's auth stuff uses this user name here
            // to determine if it has a credential already or if it can use one from the data
            // store. Setting it to the user name would result in only 1 credential for any
            // given user. The way I want it to work is every time I try to do an auth it
            // will re-auth the whole process. 
            //return controller.User.Identity.GetUserName();
            return _username;
        }

        public override IAuthorizationCodeFlow Flow
        {
            get
            {
                if (_flowInstance == null)
                {
                    _flowInstance =
                        new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
                        {
                            ClientSecrets = new ClientSecrets
                            {
                                ClientId = Setting<string>("GoogClientID"),
                                ClientSecret = Setting<string>("GoogClientSecret")
                            },
                            Scopes = new[]
                            {
                                PeopleService.Scope.Contacts,
                                PeopleService.Scope.UserinfoEmail,
                                PeopleService.Scope.UserinfoProfile
                            },
                            // This is pretty bad, we would not ordinarily store this info for a web app
                            // in a FileDataStore. However, for purposes of this project this will work;
                            // implementing a proper database-based DataStore is more work than I want
                            // to do right now.
                            DataStore = new FileDataStore("ContactMerger")
                        });
                }
                return _flowInstance;
            }
        }

        private static T Setting<T>(string name)
        {
            string value = ConfigurationManager.AppSettings[name];

            if (value == null)
            {
                throw new Exception($"Could not find setting '{name}',");
            }

            return (T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture);
        }
    }
}