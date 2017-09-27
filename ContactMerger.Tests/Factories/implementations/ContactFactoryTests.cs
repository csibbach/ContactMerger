using System.Collections.Generic;
using ContactMerger.Factories.implementations;
using ContactMerger.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContactMerger.Tests.Factories.implementations
{
    [TestClass]
    public class ContactFactoryTests
    {
        [TestMethod]
        public void CreateContactTest()
        {
            // Arrange
            var factory = new ContactFactory();

            // Act
            var contact = factory.CreateContact("Phoebe", "Yuriko", "phoebe@cute.com");

            // Assert
            Assert.IsNotNull(contact);
            Assert.AreEqual("Phoebe", contact.FirstName);
            Assert.AreEqual("Yuriko", contact.LastName);
            Assert.AreEqual("phoebe@cute.com", contact.Email);
        }

        [TestMethod]
        public void CreateContactAccountTest()
        {
            // Arrange
            var factory = new ContactFactory();

            // Act
            var contactAccount = factory.CreateContactAccount("account1@test.com", EContactAccountType.Google);

            // Assert
            Assert.IsNotNull(contactAccount);
            Assert.AreEqual("account1@test.com", contactAccount.AccountEmail);
            Assert.AreEqual(EContactAccountType.Google, contactAccount.ContactAccountType);
        }

        [TestMethod]
        public void CreateContactListTest()
        {
            // Arrange
            var factory = new ContactFactory();

            // Act
            var contactList = factory.CreateContactList("account1@test.com", EContactAccountType.Google);

            // Assert
            Assert.IsNotNull(contactList);
            Assert.AreEqual("account1@test.com", contactList.Account.AccountEmail);
            Assert.AreEqual(EContactAccountType.Google, contactList.Account.ContactAccountType);
            Assert.IsNotNull(contactList.Contacts);
        }

        [TestMethod]
        public void CreateContactSetTest()
        {
            // Arrange
            var factory = new ContactFactory();
            
            var contactAccount = new ContactAccount
            {
                AccountEmail = "account1@test.com",
                ContactAccountType = EContactAccountType.Google
            };

            var accounts = new List<ContactAccount>
            {
                contactAccount
            };

        // Act
        var contactSet = factory.CreateContactSet(accounts);

            // Assert
            Assert.IsNotNull(contactSet);
            Assert.AreEqual(1, contactSet.ContactGrid.Count);
            Assert.IsNotNull(contactSet.ContactGrid["account1@test.com"]);
        }

        [TestMethod]
        public void CreateContactRelationshipTest()
        {
            // Arrange
            var factory = new ContactFactory();

            var contact = new Contact
            {
                Email = "phoebe@cute.com",
                FirstName = "Phoebe",
                LastName = "Yuriko"
            };

            // Act
            var contactRelationship = factory.CreateContactRelationship(contact, "account1@test.com");

            // Assert
            Assert.IsNotNull(contactRelationship);
            Assert.AreEqual("Phoebe", contactRelationship.FirstName);
            Assert.AreEqual("Yuriko", contactRelationship.LastName);
            Assert.AreEqual("phoebe@cute.com", contactRelationship.Email);
            Assert.AreEqual(true, contactRelationship.FirstNameMatches);
            Assert.AreEqual(true, contactRelationship.LastNameMatches);
            Assert.AreEqual(true, contactRelationship.EmailMatches);
            Assert.AreEqual(true, contactRelationship.ContactExists);
            Assert.AreEqual("account1@test.com", contactRelationship.AccountEmail);
        }

        [TestMethod]
        public void CreateEmptyContactRelationshipTest()
        {
            // Arrange
            var factory = new ContactFactory();

            // Act
            var contactRelationship = factory.CreateEmptyContactRelationship("account1@test.com");

            // Assert
            Assert.IsNotNull(contactRelationship);
            Assert.AreEqual("", contactRelationship.FirstName);
            Assert.AreEqual("", contactRelationship.LastName);
            Assert.AreEqual("", contactRelationship.Email);
            Assert.AreEqual(true, contactRelationship.FirstNameMatches);
            Assert.AreEqual(true, contactRelationship.LastNameMatches);
            Assert.AreEqual(true, contactRelationship.EmailMatches);
            Assert.AreEqual(false, contactRelationship.ContactExists);
            Assert.AreEqual("account1@test.com", contactRelationship.AccountEmail);
        }
    }
}