using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ContactMerger.Engines.implementations;
using ContactMerger.Factories.implementations;
using ContactMerger.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContactMerger.Tests.Engines.implementations
{
    [TestClass]
    public class ContactMatchingEngineTests
    {
        // Small concession to reality here. I need a way to quickly make some of these
        // models for testing purposes, and it's also a requirement for the engine itself.
        // Proper usage says I need to create a mock of it for the test, even if I use it
        // directly for building the test itself. For model factories, I have found this
        // to be a fool's errand, however. Building the mock is more difficult (at least with
        // Moq) and more prone to failure for the test than the actual implementation. So
        // in this case I'm going to use the same object, and know I'm OK, especially as this
        // implementation is fully tested anyway.
        private readonly ContactFactory _contactFactory = new ContactFactory();

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task MergeContactListsNullParamsTest()
        {
            // Arrange
            var engine = new ContactMatchingEngine(_contactFactory);

            // Act
            await engine.MergeContactLists(null);

            // Assert
        }

        [TestMethod]
        public async Task MergeContactListsNoListsTest()
        {
            // Arrange
            var engine = new ContactMatchingEngine(_contactFactory);

            // Act
            var set = await engine.MergeContactLists(new List<ContactList>());

            // Assert
            Assert.IsNotNull(set);
            Assert.AreEqual(0, set.Accounts.Count);
            Assert.AreEqual(0, set.Relationships.Count);
        }

        [TestMethod]
        public async Task MergeContactListsSingleListTest()
        {
            // Arrange
            var list = _contactFactory.CreateContactList("Account1@test.com", EContactAccountType.Google);
            var contact = _contactFactory.CreateContact("foo", "bar", "baz");
            list.Contacts.Add(contact);
            var engine = new ContactMatchingEngine(_contactFactory);

            // Act
            var set = await engine.MergeContactLists(new List<ContactList>{list});

            // Assert
            Assert.IsNotNull(set);
            Assert.AreEqual(1, set.Accounts.Count);
            Assert.AreEqual("Account1@test.com", set.Accounts[0].AccountEmail);
            Assert.AreEqual(EContactAccountType.Google, set.Accounts[0].ContactAccountType);
            Assert.AreEqual(1, set.Relationships.Count);
            Assert.AreEqual(1, set.Relationships[0].ContactAccountMap.Count);
            Assert.AreEqual(contact, set.Relationships[0].Contact);
            Assert.AreEqual("Account1@test.com", set.Relationships[0].ContactAccountMap[0]);
        }

        [TestMethod]
        public async Task MergeContactListsSingleListMultipleContactTest()
        {
            // Arrange
            var list = _contactFactory.CreateContactList("Account1@test.com", EContactAccountType.Google);
            var contact1 = _contactFactory.CreateContact("foo", "bar", "baz");
            var contact2 = _contactFactory.CreateContact("zort", "meep", "yarg");
            list.Contacts.Add(contact1);
            list.Contacts.Add(contact2);
            var engine = new ContactMatchingEngine(_contactFactory);

            // Act
            var set = await engine.MergeContactLists(new List<ContactList> { list });

            // Assert
            Assert.IsNotNull(set);
            Assert.AreEqual(1, set.Accounts.Count);
            Assert.AreEqual("Account1@test.com", set.Accounts[0].AccountEmail);
            Assert.AreEqual(EContactAccountType.Google, set.Accounts[0].ContactAccountType);
            
            Assert.AreEqual(2, set.Relationships.Count);
            Assert.AreEqual(1, set.Relationships[0].ContactAccountMap.Count);
            Assert.AreEqual(1, set.Relationships[1].ContactAccountMap.Count);
            Assert.AreEqual(contact1, set.Relationships[0].Contact);
            Assert.AreEqual(contact2, set.Relationships[1].Contact);
            Assert.AreEqual("Account1@test.com", set.Relationships[0].ContactAccountMap[0]);
            Assert.AreEqual("Account1@test.com", set.Relationships[1].ContactAccountMap[0]);
        }

        [TestMethod]
        public async Task MergeContactListsMultipleListTest()
        {
            // Arrange
            var list1 = _contactFactory.CreateContactList("Account1@test.com", EContactAccountType.Google);
            var list2 = _contactFactory.CreateContactList("Account2@test.com", EContactAccountType.Facebook);

            var contact1 = _contactFactory.CreateContact("foo", "bar", "baz");
            var contact2 = _contactFactory.CreateContact("zort", "meep", "yarg");
            var contact3 = _contactFactory.CreateContact("Foo", "  bar", "baz ");

            list1.Contacts.Add(contact1);
            list2.Contacts.Add(contact2);
            list2.Contacts.Add(contact3);

            var engine = new ContactMatchingEngine(_contactFactory);

            // Act
            var set = await engine.MergeContactLists(new List<ContactList> { list1, list2 });

            // Assert
            // Account Asserts
            Assert.IsNotNull(set);
            Assert.AreEqual(2, set.Accounts.Count);
            Assert.AreEqual("Account1@test.com", set.Accounts[0].AccountEmail);
            Assert.AreEqual(EContactAccountType.Google, set.Accounts[0].ContactAccountType);
            Assert.AreEqual("Account2@test.com", set.Accounts[1].AccountEmail);
            Assert.AreEqual(EContactAccountType.Facebook, set.Accounts[1].ContactAccountType);
            Assert.AreEqual(2, set.Relationships.Count);

            // First relationship should be foo
            Assert.AreEqual(2, set.Relationships[0].ContactAccountMap.Count);
            Assert.AreEqual(contact1, set.Relationships[0].Contact);
            Assert.AreEqual("Account1@test.com", set.Relationships[0].ContactAccountMap[0]);
            Assert.AreEqual("Account2@test.com", set.Relationships[0].ContactAccountMap[1]);

            // First relationship should be yarg
            Assert.AreEqual(1, set.Relationships[1].ContactAccountMap.Count);
            Assert.AreEqual(contact2, set.Relationships[1].Contact);
            Assert.AreEqual("Account2@test.com", set.Relationships[1].ContactAccountMap[0]);
        }

        [TestMethod]
        public async Task ContactsMatchTest()
        {
            // Arrange
            var contact1 = _contactFactory.CreateContact("foo", "Bar", "baz");
            var contact2 = _contactFactory.CreateContact("Foo", "Bar ", " baz");
            var engine = new ContactMatchingEngine(_contactFactory);

            // Act
            var matching = await engine.ContactsMatch(contact1, contact2);

            // Assert
            Assert.IsTrue(matching);
        }

        [TestMethod]
        public async Task ContactsMatchFirstNameDifferentTest()
        {
            // Arrange
            var contact1 = _contactFactory.CreateContact("foo", "Bar", "baz");
            var contact2 = _contactFactory.CreateContact("Fooo", "Bar", "baz");
            var engine = new ContactMatchingEngine(_contactFactory);

            // Act
            var matching = await engine.ContactsMatch(contact1, contact2);

            // Assert
            Assert.IsFalse(matching);
        }

        [TestMethod]
        public async Task ContactsMatchLastNameDifferentTest()
        {
            // Arrange
            var contact1 = _contactFactory.CreateContact("foo", "Bar", "baz");
            var contact2 = _contactFactory.CreateContact("foo", "Barr", "baz");
            var engine = new ContactMatchingEngine(_contactFactory);

            // Act
            var matching = await engine.ContactsMatch(contact1, contact2);

            // Assert
            Assert.IsFalse(matching);
        }

        [TestMethod]
        public async Task ContactsMatchEmailDifferentTest()
        {
            // Arrange
            var contact1 = _contactFactory.CreateContact("foo", "Bar", "baz");
            var contact2 = _contactFactory.CreateContact("foo", "Bar", "ba");
            var engine = new ContactMatchingEngine(_contactFactory);

            // Act
            var matching = await engine.ContactsMatch(contact1, contact2);

            // Assert
            Assert.IsFalse(matching);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task ContactsMatchNullParamsTest1()
        {
            // Arrange
            var contact = _contactFactory.CreateContact("foo", "Bar", "baz");
            var engine = new ContactMatchingEngine(_contactFactory);

            // Act
            await engine.ContactsMatch(contact, null);

            // Assert
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task ContactsMatchNullParamsTest2()
        {
            // Arrange
            var contact = _contactFactory.CreateContact("foo", "Bar", "baz");
            var engine = new ContactMatchingEngine(_contactFactory);

            // Act
            await engine.ContactsMatch(null, contact);

            // Assert
        }

    }
}