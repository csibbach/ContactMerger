using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ContactMerger.DataProviders.contracts;
using ContactMerger.Utility;
using Google.Apis.Auth.OAuth2.Mvc;
using Google.Apis.People.v1;
using Google.Apis.Services;
using Microsoft.AspNet.Identity;

namespace ContactMerger.Controllers
{
    public class ContactAccountController : Controller
    {
        private readonly IFlowMetadataFactory _flowMetadataFactory;
        private readonly IGoogleCredentialProvider _googleCredentialProvider;

        public ContactAccountController(IFlowMetadataFactory flowMetadataFactory,
            IGoogleCredentialProvider googleCredentialProvider)
        {
            _flowMetadataFactory = flowMetadataFactory;
            _googleCredentialProvider = googleCredentialProvider;
        }

        // This endpoint exists only to walk the user through giving us authentication
        // with google to access their data. Each time you call this it stores the credential
        // under the authorized user in the credential provider, so that it can be used in other
        // calls.
        [Authorize]
        public async Task<ActionResult> AddContactAccount(CancellationToken cancellationToken)
        {
            // Create a new Authorization Flow- should have a factory for this for testing purposes
            var result = await new AuthorizationCodeMvcApp(this, _flowMetadataFactory.CreateFlowMetadata()).
                AuthorizeAsync(cancellationToken);

            // Redirect if necessary
            if (result.Credential == null)
                return new RedirectResult(result.RedirectUri);

            _googleCredentialProvider.SaveCredential(User.Identity.GetUserName(), result.Credential);


            //var peopleService = new PeopleService(new BaseClientService.Initializer
            //{
            //    HttpClientInitializer = result.Credential,
            //    ApplicationName = "ClientMerger"
            //});

            //var listReq = driveService.Files.List();
            //listReq.Fields = "items/title,items/id,items/createdDate,items/downloadUrl,items/exportLinks";
            //var list = await listReq.ExecuteAsync();
            //var items =
            //(from file in list.Items
            //    select new FileModel
            //    {
            //        Title = file.Title,
            //        Id = file.Id,
            //        CreatedDate = file.CreatedDate,
            //        DownloadUrl = file.DownloadUrl ??
            //                      (file.ExportLinks != null ? file.ExportLinks["application/pdf"] : null),
            //    }).OrderBy(f => f.Title).ToList();
            return View();
        }
    }
}