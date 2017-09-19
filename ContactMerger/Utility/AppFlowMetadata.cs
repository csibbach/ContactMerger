using System;
using System.Configuration;
using System.Globalization;
using System.Web.Mvc;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Mvc;
using Google.Apis.People.v1;
using Google.Apis.Util.Store;
using Microsoft.AspNet.Identity;

namespace ContactMerger.Utility
{
    public class AppFlowMetadata : FlowMetadata
    {
        private static readonly IAuthorizationCodeFlow FlowInstance =
            new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = new ClientSecrets
                {
                    ClientId = Setting<string>("GoogClientID"),
                    ClientSecret = Setting<string>("GoogClientSecret")
                },
                Scopes = new[] { PeopleService.Scope.Contacts, PeopleService.Scope.UserEmailsRead, PeopleService.Scope.UserinfoEmail },
                // This is pretty bad, we would not ordinarily store this info for a web app
                // in a FileDataStore. However, for purposes of this project this will work;
                // implementing a proper database-based DataStore is more work than I want
                // to do right now.
                DataStore = new FileDataStore("ContactMerger") // This
            });

        public override string GetUserId(Controller controller)
        {
            return controller.User.Identity.GetUserName();
        }

        public override IAuthorizationCodeFlow Flow
        {
            get { return FlowInstance; }
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