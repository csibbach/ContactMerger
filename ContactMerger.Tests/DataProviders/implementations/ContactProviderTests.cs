using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ContactMerger.DataProviders.implementations;
using ContactMerger.Factories.implementations;
using ContactMerger.Models;
using ContactMerger.Tests.Mocks;
using Google.Apis.People.v1.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContactMerger.Tests.DataProviders.implementations
{
    [TestClass]
    public class ContactProviderTests
    {
        private readonly ContactFactory _contactFactory = new ContactFactory();

        [TestMethod]
        public async Task GetContactsEmptyListTest()
        {
            // Arrange
            var credentialProviderMock = MyMockFactory.CreateGoogleCredentialProviderMock();
            var googleApiConnectorMock = MyMockFactory.CreateGoogleApiConnectorMock();
            var provider = new ContactProvider(credentialProviderMock.Object, 
                _contactFactory,
                googleApiConnectorMock.Object);

            // Act
            var result = await provider.GetContacts(MyMockFactory.Username, MyMockFactory.CredentialEmail1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(MyMockFactory.CredentialEmail1, result.Account.AccountEmail);
            Assert.AreEqual(EContactAccountType.Google, result.Account.ContactAccountType);
            Assert.AreEqual(0, result.Contacts.Count);
        }

        [TestMethod]
        public async Task GetContactsSingleContactTest()
        {
            // Arrange
            var credentialProviderMock = MyMockFactory.CreateGoogleCredentialProviderMock();
            var personList = new List<Person>
            {
                MyMockFactory.CreatePerson("Phoebe", "Yuriko", "phoebe@cute.com")
            };

            var googleApiConnectorMock = MyMockFactory.CreateGoogleApiConnectorMock(personList);
            var provider = new ContactProvider(credentialProviderMock.Object,
                _contactFactory,
                googleApiConnectorMock.Object);

            // Act
            var result = await provider.GetContacts(MyMockFactory.Username, MyMockFactory.CredentialEmail1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(MyMockFactory.CredentialEmail1, result.Account.AccountEmail);
            Assert.AreEqual(EContactAccountType.Google, result.Account.ContactAccountType);
            Assert.AreEqual(1, result.Contacts.Count);
            Assert.AreEqual("Phoebe", result.Contacts[0].FirstName);
            Assert.AreEqual("Yuriko", result.Contacts[0].LastName);
            Assert.AreEqual("phoebe@cute.com", result.Contacts[0].Email);
        }

        [TestMethod]
        public async Task GetContactsNullFirstNameTest()
        {
            // Arrange
            var credentialProviderMock = MyMockFactory.CreateGoogleCredentialProviderMock();
            var personList = new List<Person>
            {
                MyMockFactory.CreatePerson(null, "Yuriko", "phoebe@cute.com")
            };

            // Let's do this one with credential2 to test our mock
            var googleApiConnectorMock = MyMockFactory.CreateGoogleApiConnectorMock(null, personList);
            var provider = new ContactProvider(credentialProviderMock.Object,
                _contactFactory,
                googleApiConnectorMock.Object);

            // Act
            var result = await provider.GetContacts(MyMockFactory.Username, MyMockFactory.CredentialEmail2);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(MyMockFactory.CredentialEmail2, result.Account.AccountEmail);
            Assert.AreEqual(EContactAccountType.Google, result.Account.ContactAccountType);
            Assert.AreEqual(1, result.Contacts.Count);
            Assert.AreEqual("", result.Contacts[0].FirstName);
            Assert.AreEqual("Yuriko", result.Contacts[0].LastName);
            Assert.AreEqual("phoebe@cute.com", result.Contacts[0].Email);
        }

        [TestMethod]
        public async Task GetContactsNullLastNameTest()
        {
            // Arrange
            var credentialProviderMock = MyMockFactory.CreateGoogleCredentialProviderMock();
            var personList = new List<Person>
            {
                MyMockFactory.CreatePerson("Phoebe", "", "phoebe@cute.com")
            };

            var googleApiConnectorMock = MyMockFactory.CreateGoogleApiConnectorMock(personList);
            var provider = new ContactProvider(credentialProviderMock.Object,
                _contactFactory,
                googleApiConnectorMock.Object);

            // Act
            var result = await provider.GetContacts(MyMockFactory.Username, MyMockFactory.CredentialEmail1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(MyMockFactory.CredentialEmail1, result.Account.AccountEmail);
            Assert.AreEqual(EContactAccountType.Google, result.Account.ContactAccountType);
            Assert.AreEqual(1, result.Contacts.Count);
            Assert.AreEqual("Phoebe", result.Contacts[0].FirstName);
            Assert.AreEqual("", result.Contacts[0].LastName);
            Assert.AreEqual("phoebe@cute.com", result.Contacts[0].Email);
        }

        [TestMethod]
        public async Task GetContactsNullEmailTest()
        {
            // Arrange
            var credentialProviderMock = MyMockFactory.CreateGoogleCredentialProviderMock();
            var personList = new List<Person>
            {
                MyMockFactory.CreatePerson("Phoebe", "Yuriko")
            };

            var googleApiConnectorMock = MyMockFactory.CreateGoogleApiConnectorMock(personList);
            var provider = new ContactProvider(credentialProviderMock.Object,
                _contactFactory,
                googleApiConnectorMock.Object);

            // Act
            var result = await provider.GetContacts(MyMockFactory.Username, MyMockFactory.CredentialEmail1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(MyMockFactory.CredentialEmail1, result.Account.AccountEmail);
            Assert.AreEqual(EContactAccountType.Google, result.Account.ContactAccountType);
            Assert.AreEqual(1, result.Contacts.Count);
            Assert.AreEqual("Phoebe", result.Contacts[0].FirstName);
            Assert.AreEqual("Yuriko", result.Contacts[0].LastName);
            Assert.AreEqual("", result.Contacts[0].Email);
        }

        [TestMethod]
        public async Task AddContactTest()
        {
            // Arrange
            var credentialProviderMock = MyMockFactory.CreateGoogleCredentialProviderMock();
            var googleApiConnectorMock = MyMockFactory.CreateGoogleApiConnectorMock();
            var provider = new ContactProvider(credentialProviderMock.Object,
                _contactFactory,
                googleApiConnectorMock.Object);

            // Act
            var result = await provider.AddContact(MyMockFactory.Username, MyMockFactory.CredentialEmail1, "Phoebe", "Yuriko", "phoebe@cute.com");

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task AddContactBadCredentialTest()
        {
            // Arrange
            var credentialProviderMock = MyMockFactory.CreateGoogleCredentialProviderMock();
            var googleApiConnectorMock = MyMockFactory.CreateGoogleApiConnectorMock();
            var provider = new ContactProvider(credentialProviderMock.Object,
                _contactFactory,
                googleApiConnectorMock.Object);

            // Act
            await provider.AddContact(MyMockFactory.Username, MyMockFactory.BadCredentialEmail, "Phoebe", "Yuriko", "phoebe@cute.com");
            
        }
    }
}