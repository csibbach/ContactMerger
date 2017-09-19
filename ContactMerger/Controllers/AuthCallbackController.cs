using ContactMerger.Utility;

namespace ContactMerger.Controllers
{
    public class AuthCallbackController : Google.Apis.Auth.OAuth2.Mvc.Controllers.AuthCallbackController
    {
        private readonly IFlowMetadataFactory _flowMetadataFactory;

        public AuthCallbackController(IFlowMetadataFactory flowMetadataFactory)
        {
            _flowMetadataFactory = flowMetadataFactory;
        }

        protected override Google.Apis.Auth.OAuth2.Mvc.FlowMetadata FlowData
        {
            get { return _flowMetadataFactory.CreateFlowMetadata(); }
        }
    }
}