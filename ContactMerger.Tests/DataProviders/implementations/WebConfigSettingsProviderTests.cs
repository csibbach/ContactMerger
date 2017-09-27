using ContactMerger.DataProviders.implementations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContactMerger.Tests.DataProviders.implementations
{
    [TestClass]
    public class WebConfigSettingsProviderTests
    {
        // I need fakes to test this class!
        [TestMethod]
        public void GetGoogleClientIdTest()
        {
            // Arrange
            var provider = new WebConfigSettingsProvider();

            // Act

            // Assert
            Assert.Inconclusive();
        }

        [TestMethod]
        public void GetGoogleClientSecretTest()
        {
            Assert.Inconclusive();
        }
    }
}