using ContactMerger.Factories.contracts;
using ContactMerger.Utility;

namespace ContactMerger.Factories.implementations
{
    public class FlowMetadataFactory: IFlowMetadataFactory
    {
        private int _accountCount;

        public AppFlowMetadata CreateFlowMetadata()
        {
            return new AppFlowMetadata($"Account{_accountCount}");
        }

        public void RequestNewAccount()
        {
            _accountCount++;
        }
    }
}