using ContactMerger.Factories.implementations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContactMerger.Tests.Factories.implementations
{
    [TestClass]
    public class FlowMetadataFactoryTests
    {
        [TestMethod]
        public void CreateFlowMetadataTest()
        {
            // Arrange
            var factory = new FlowMetadataFactory();

            // Act
            var flowMetadata = factory.CreateFlowMetadata();

            // Assert
            Assert.AreEqual("Account0", flowMetadata.GetUserId(null));
        }

        [TestMethod]
        public void RequestNewAccountTest()
        {
            // Arrange
            var factory = new FlowMetadataFactory();

            // Act
            factory.RequestNewAccount();
            var flowMetadata = factory.CreateFlowMetadata();

            // Assert
            Assert.AreEqual("Account1", flowMetadata.GetUserId(null));
        }
    }
}