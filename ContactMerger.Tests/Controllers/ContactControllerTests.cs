using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ContactMerger.Controllers;
using ContactMerger.DataProviders.contracts;
using ContactMerger.Engines.contracts;
using ContactMerger.Factories.implementations;
using ContactMerger.Models;
using ContactMerger.Tests.Mocks;
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
            contactProviderMock.Setup(x => x.GetContacts("username", MyMockFactory.CredentialEmail1))
                .Returns(Task.FromResult(_contactFactory.CreateContactList(MyMockFactory.CredentialEmail1, EContactAccountType.Google)));

            var credentialProviderMock = MyMockFactory.CreateGoogleCredentialProviderMock();

            // MatchingEngine
            var contactSet = _contactFactory.CreateContactSet(new List<ContactAccount>());
            var contactMatchingEngineMock = new Mock<IContactMatchingEngine>();
            contactMatchingEngineMock.Setup(x => x.MergeContactLists(It.IsAny<IList<ContactList>>()))
                .Returns(Task.FromResult(contactSet));

            var controller = new ContactController(contactProviderMock.Object,
                credentialProviderMock.Object,
                contactMatchingEngineMock.Object);
            MyMockFactory.SetUsernameOnController(controller);

            // Act
            var result = await controller.GetContactSet();

            // Assert
            Assert.IsNotNull(result);
            credentialProviderMock.Verify(x => x.GetCredentials("username"), Times.Once);
            contactProviderMock.Verify(x => x.GetContacts("username", MyMockFactory.CredentialEmail1), Times.Once);
            contactProviderMock.Verify(x => x.GetContacts("username", MyMockFactory.CredentialEmail2), Times.Once);
        }

        [TestMethod]
        public async Task AddContactsTest()
        {
            // Arrange
            var contactProviderMock = new Mock<IContactProvider>();
            contactProviderMock.Setup(x => x.AddContact("username", MyMockFactory.CredentialEmail2, "Phoebe", "Yuriko", "phoebe@cute.com"))
                .Returns(Task.FromResult("blah"));

            var credentialProviderMock = MyMockFactory.CreateGoogleCredentialProviderMock();

            // MatchingEngine
            var contactMatchingEngineMock = new Mock<IContactMatchingEngine>();
        
            var controller = new ContactController(contactProviderMock.Object,
                credentialProviderMock.Object,
                contactMatchingEngineMock.Object);
            MyMockFactory.SetUsernameOnController(controller);

            var request = new AddContactRequest
            {
                AccountEmail = MyMockFactory.CredentialEmail1,
                Email = "phoebe@cute.com",
                FirstName = "Phoebe",
                LastName = "Yuriko"
            };

            // Act
            var response = await controller.AddContacts(request);

            // Assert
            Assert.IsNotNull(response);
            credentialProviderMock.Verify(x => x.GetCredentials("username"), Times.Once);
            contactProviderMock.Verify(x => x.AddContact("username", MyMockFactory.CredentialEmail2, request.FirstName, request.LastName, request.Email), Times.Once);
        }

    }
}