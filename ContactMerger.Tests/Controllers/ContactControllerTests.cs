using System.Collections.Generic;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using ContactMerger.Controllers;
using ContactMerger.DataProviders.contracts;
using ContactMerger.Engines.contracts;
using ContactMerger.Factories.implementations;
using ContactMerger.Models;
using Google.Apis.Auth.OAuth2;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ContactMerger.Tests.Controllers
{
    [TestClass]
    public class ContactControllerTests
    {
        private readonly ContactFactory _contactFactory = new ContactFactory();

        [TestMethod]
        public async Task GetTest()
        {
            // Arrange
            var contactProviderMock = new Mock<IContactProvider>();
            contactProviderMock.Setup(x => x.GetContacts("username", "account1@google.com"))
                .Returns(Task.FromResult(_contactFactory.CreateContactList("account1@google.com", EContactAccountType.Google)));

            var credentialProviderMock = new Mock<IGoogleCredentialProvider>();

            // Stored credentials
            var storedCredentials = new Dictionary<string, UserCredential>
            {
                {"account1@google.com", null}
            };
            credentialProviderMock.Setup(x => x.GetCredentials("username"))
                .Returns(Task.FromResult(storedCredentials as IDictionary<string, UserCredential>)); // Wow that sucks

            // MatchingEngine
            var contactSet = _contactFactory.CreateContactSet(null);
            var contactMatchingEngineMock = new Mock<IContactMatchingEngine>();
            contactMatchingEngineMock.Setup(x => x.MergeContactLists(It.IsAny<IList<ContactList>>()))
                .Returns(Task.FromResult(contactSet));

            var identity = new GenericIdentity("username");
            Thread.CurrentPrincipal = new GenericPrincipal(identity, null);
            var controller = new ContactController(contactProviderMock.Object,
                credentialProviderMock.Object,
                contactMatchingEngineMock.Object);


            // Act
            var set = await controller.GetContactSet();

            // Assert
            Assert.AreEqual(contactSet, set);
        }

        [TestMethod]
        public void GetTest1()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void PostTest()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void PutTest()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void DeleteTest()
        {
            Assert.Fail();
        }
    }
}