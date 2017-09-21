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

        private const string Account1 = "Account1@test.com";
        private const string Account2 = "Account2@test.com";

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
            Assert.AreEqual(0, set.ContactGrid.Count);
        }

        [TestMethod]
        public async Task MergeContactListsSingleListTest()
        {
            // Arrange
            var list = _contactFactory.CreateContactList(Account1, EContactAccountType.Google);
            var contact = _contactFactory.CreateContact("foo", "bar", "baz");
            list.Contacts.Add(contact);
            var engine = new ContactMatchingEngine(_contactFactory);

            // Act
            var set = await engine.MergeContactLists(new List<ContactList>{list});

            // Assert
            Assert.IsNotNull(set);
            Assert.AreEqual(1, set.ContactGrid.Count);
            Assert.IsTrue(set.ContactGrid.ContainsKey(Account1));
            Assert.AreEqual(1, set.ContactGrid[Account1].Count);

            var relationship1 = set.ContactGrid[Account1][0];
            Assert.AreEqual("foo", relationship1.FirstName);
            Assert.AreEqual("bar", relationship1.LastName);
            Assert.AreEqual("baz", relationship1.Email);
            // Status bits
            Assert.AreEqual(true, relationship1.FirstNameMatches);
            Assert.AreEqual(true, relationship1.LastNameMatches);
            Assert.AreEqual(true, relationship1.EmailMatches);
            Assert.AreEqual(true, relationship1.ContactExists);
            Assert.AreEqual(Account1, relationship1.AccountEmail);
        }

        [TestMethod]
        public async Task MergeContactListsSingleListMultipleContactTest()
        {
            // Arrange
            var list = _contactFactory.CreateContactList(Account1, EContactAccountType.Google);
            var contact1 = _contactFactory.CreateContact("foo", "bar", "baz");
            var contact2 = _contactFactory.CreateContact("zort", "meep", "yarg");
            list.Contacts.Add(contact1);
            list.Contacts.Add(contact2);
            var engine = new ContactMatchingEngine(_contactFactory);

            // Act
            var set = await engine.MergeContactLists(new List<ContactList> { list });

            // Assert
            Assert.IsNotNull(set);
            Assert.AreEqual(1, set.ContactGrid.Count);
            Assert.IsTrue(set.ContactGrid.ContainsKey(Account1));
            Assert.AreEqual(2, set.ContactGrid[Account1].Count);

            // First contact
            var relationship1 = set.ContactGrid[Account1][0];
            Assert.AreEqual("foo", relationship1.FirstName);
            Assert.AreEqual("bar", relationship1.LastName);
            Assert.AreEqual("baz", relationship1.Email);
            // Status bits
            Assert.AreEqual(true, relationship1.FirstNameMatches);
            Assert.AreEqual(true, relationship1.LastNameMatches);
            Assert.AreEqual(true, relationship1.EmailMatches);
            Assert.AreEqual(true, relationship1.ContactExists);
            Assert.AreEqual(Account1, relationship1.AccountEmail);

            // Second contact
            var relationship2 = set.ContactGrid[Account1][1];
            Assert.AreEqual("zort", relationship2.FirstName);
            Assert.AreEqual("meep", relationship2.LastName);
            Assert.AreEqual("yarg", relationship2.Email);
            // Status bits
            Assert.AreEqual(true, relationship2.FirstNameMatches);
            Assert.AreEqual(true, relationship2.LastNameMatches);
            Assert.AreEqual(true, relationship2.EmailMatches);
            Assert.AreEqual(true, relationship2.ContactExists);
            Assert.AreEqual(Account1, relationship1.AccountEmail);
        }

        [TestMethod]
        public async Task MergeContactListsMultipleListFullMatchesTest()
        {
            // Arrange
            var list1 = _contactFactory.CreateContactList(Account1, EContactAccountType.Google);
            var list2 = _contactFactory.CreateContactList(Account2, EContactAccountType.Facebook);

            var contact1 = _contactFactory.CreateContact("foo", "bar", "baz");
            var contact2 = _contactFactory.CreateContact("zort", "meep", "yarg");
            var contact3 = _contactFactory.CreateContact("foo", "bar", "baz");

            list1.Contacts.Add(contact1);
            list2.Contacts.Add(contact2);
            list2.Contacts.Add(contact3);

            var engine = new ContactMatchingEngine(_contactFactory);

            // Act
            var set = await engine.MergeContactLists(new List<ContactList> { list1, list2 });

            // Assert
            // Account Asserts, 2 columns with 2 rows
            Assert.IsNotNull(set);
            Assert.AreEqual(2, set.ContactGrid.Count);
            Assert.IsTrue(set.ContactGrid.ContainsKey(Account1));
            Assert.IsTrue(set.ContactGrid.ContainsKey(Account2));
            Assert.AreEqual(2, set.ContactGrid[Account1].Count);
            Assert.AreEqual(2, set.ContactGrid[Account2].Count);

            // Column 1, contact 1
            var relationship11 = set.ContactGrid[Account1][0];
            Assert.AreEqual("foo", relationship11.FirstName);
            Assert.AreEqual("bar", relationship11.LastName);
            Assert.AreEqual("baz", relationship11.Email);
            // Status bits
            Assert.AreEqual(true, relationship11.FirstNameMatches);
            Assert.AreEqual(true, relationship11.LastNameMatches);
            Assert.AreEqual(true, relationship11.EmailMatches);
            Assert.AreEqual(true, relationship11.ContactExists);
            Assert.AreEqual(Account1, relationship11.AccountEmail);

            // Column 1, contact 2
            var relationship12 = set.ContactGrid[Account1][1];
            Assert.AreEqual("", relationship12.FirstName);
            Assert.AreEqual("", relationship12.LastName);
            Assert.AreEqual("", relationship12.Email);
            // Status bits
            Assert.AreEqual(true, relationship12.FirstNameMatches);
            Assert.AreEqual(true, relationship12.LastNameMatches);
            Assert.AreEqual(true, relationship12.EmailMatches);
            Assert.AreEqual(false, relationship12.ContactExists);
            Assert.AreEqual(Account1, relationship12.AccountEmail);

            // Column 2, contact 1
            var relationship21 = set.ContactGrid[Account2][0];
            Assert.AreEqual("foo", relationship21.FirstName);
            Assert.AreEqual("bar", relationship21.LastName);
            Assert.AreEqual("baz", relationship21.Email);
            // Status bits
            Assert.AreEqual(true, relationship21.FirstNameMatches);
            Assert.AreEqual(true, relationship21.LastNameMatches);
            Assert.AreEqual(true, relationship21.EmailMatches);
            Assert.AreEqual(true, relationship21.ContactExists);
            Assert.AreEqual(Account2, relationship21.AccountEmail);

            // Column 2, contact 2
            var relationship22 = set.ContactGrid[Account2][1];
            Assert.AreEqual("zort", relationship22.FirstName);
            Assert.AreEqual("meep", relationship22.LastName);
            Assert.AreEqual("yarg", relationship22.Email);
            // Status bits
            Assert.AreEqual(true, relationship22.FirstNameMatches);
            Assert.AreEqual(true, relationship22.LastNameMatches);
            Assert.AreEqual(true, relationship22.EmailMatches);
            Assert.AreEqual(true, relationship22.ContactExists);
            Assert.AreEqual(Account2, relationship22.AccountEmail);
        }

        [TestMethod]
        public async Task MergeContactListsMultipleListFirstNameMismatchTest()
        {
            // Arrange
            var list1 = _contactFactory.CreateContactList(Account1, EContactAccountType.Google);
            var list2 = _contactFactory.CreateContactList(Account2, EContactAccountType.Facebook);

            var contact1 = _contactFactory.CreateContact("foo", "bar", "baz");
            var contact2 = _contactFactory.CreateContact("zort", "meep", "yarg");
            var contact3 = _contactFactory.CreateContact("blam", "bar", "baz");

            list1.Contacts.Add(contact1);
            list2.Contacts.Add(contact2);
            list2.Contacts.Add(contact3);

            var engine = new ContactMatchingEngine(_contactFactory);

            // Act
            var set = await engine.MergeContactLists(new List<ContactList> { list1, list2 });

            // Assert
            // Account Asserts, 2 columns with 2 rows
            Assert.IsNotNull(set);
            Assert.AreEqual(2, set.ContactGrid.Count);
            Assert.IsTrue(set.ContactGrid.ContainsKey(Account1));
            Assert.IsTrue(set.ContactGrid.ContainsKey(Account2));
            Assert.AreEqual(2, set.ContactGrid[Account1].Count);
            Assert.AreEqual(2, set.ContactGrid[Account2].Count);

            // Column 1, contact 1
            var relationship11 = set.ContactGrid[Account1][0];
            Assert.AreEqual("foo", relationship11.FirstName);
            Assert.AreEqual("bar", relationship11.LastName);
            Assert.AreEqual("baz", relationship11.Email);
            // Status bits
            Assert.AreEqual(false, relationship11.FirstNameMatches);
            Assert.AreEqual(true, relationship11.LastNameMatches);
            Assert.AreEqual(true, relationship11.EmailMatches);
            Assert.AreEqual(true, relationship11.ContactExists);
            Assert.AreEqual(Account1, relationship11.AccountEmail);

            // Column 1, contact 2
            var relationship12 = set.ContactGrid[Account1][1];
            Assert.AreEqual("", relationship12.FirstName);
            Assert.AreEqual("", relationship12.LastName);
            Assert.AreEqual("", relationship12.Email);
            // Status bits
            Assert.AreEqual(true, relationship12.FirstNameMatches);
            Assert.AreEqual(true, relationship12.LastNameMatches);
            Assert.AreEqual(true, relationship12.EmailMatches);
            Assert.AreEqual(false, relationship12.ContactExists);
            Assert.AreEqual(Account1, relationship12.AccountEmail);

            // Column 2, contact 1
            var relationship21 = set.ContactGrid[Account2][0];
            Assert.AreEqual("blam", relationship21.FirstName);
            Assert.AreEqual("bar", relationship21.LastName);
            Assert.AreEqual("baz", relationship21.Email);
            // Status bits
            Assert.AreEqual(false, relationship21.FirstNameMatches);
            Assert.AreEqual(true, relationship21.LastNameMatches);
            Assert.AreEqual(true, relationship21.EmailMatches);
            Assert.AreEqual(true, relationship21.ContactExists);
            Assert.AreEqual(Account2, relationship21.AccountEmail);

            // Column 2, contact 2
            var relationship22 = set.ContactGrid[Account2][1];
            Assert.AreEqual("zort", relationship22.FirstName);
            Assert.AreEqual("meep", relationship22.LastName);
            Assert.AreEqual("yarg", relationship22.Email);
            // Status bits
            Assert.AreEqual(true, relationship22.FirstNameMatches);
            Assert.AreEqual(true, relationship22.LastNameMatches);
            Assert.AreEqual(true, relationship22.EmailMatches);
            Assert.AreEqual(true, relationship22.ContactExists);
            Assert.AreEqual(Account2, relationship22.AccountEmail);
        }

        [TestMethod]
        public async Task MergeContactListsMultipleListLastNameMismatchTest()
        {
            // Arrange
            var list1 = _contactFactory.CreateContactList(Account1, EContactAccountType.Google);
            var list2 = _contactFactory.CreateContactList(Account2, EContactAccountType.Facebook);

            var contact1 = _contactFactory.CreateContact("foo", "bar", "baz");
            var contact2 = _contactFactory.CreateContact("zort", "meep", "yarg");
            var contact3 = _contactFactory.CreateContact("foo", "blam", "baz");

            list1.Contacts.Add(contact1);
            list2.Contacts.Add(contact2);
            list2.Contacts.Add(contact3);

            var engine = new ContactMatchingEngine(_contactFactory);

            // Act
            var set = await engine.MergeContactLists(new List<ContactList> { list1, list2 });

            // Assert
            // Account Asserts, 2 columns with 2 rows
            Assert.IsNotNull(set);
            Assert.AreEqual(2, set.ContactGrid.Count);
            Assert.IsTrue(set.ContactGrid.ContainsKey(Account1));
            Assert.IsTrue(set.ContactGrid.ContainsKey(Account2));
            Assert.AreEqual(2, set.ContactGrid[Account1].Count);
            Assert.AreEqual(2, set.ContactGrid[Account2].Count);

            // Column 1, contact 1
            var relationship11 = set.ContactGrid[Account1][0];
            Assert.AreEqual("foo", relationship11.FirstName);
            Assert.AreEqual("bar", relationship11.LastName);
            Assert.AreEqual("baz", relationship11.Email);
            // Status bits
            Assert.AreEqual(true, relationship11.FirstNameMatches);
            Assert.AreEqual(false, relationship11.LastNameMatches);
            Assert.AreEqual(true, relationship11.EmailMatches);
            Assert.AreEqual(true, relationship11.ContactExists);
            Assert.AreEqual(Account1, relationship11.AccountEmail);

            // Column 1, contact 2
            var relationship12 = set.ContactGrid[Account1][1];
            Assert.AreEqual("", relationship12.FirstName);
            Assert.AreEqual("", relationship12.LastName);
            Assert.AreEqual("", relationship12.Email);
            // Status bits
            Assert.AreEqual(true, relationship12.FirstNameMatches);
            Assert.AreEqual(true, relationship12.LastNameMatches);
            Assert.AreEqual(true, relationship12.EmailMatches);
            Assert.AreEqual(false, relationship12.ContactExists);
            Assert.AreEqual(Account1, relationship12.AccountEmail);

            // Column 2, contact 1
            var relationship21 = set.ContactGrid[Account2][0];
            Assert.AreEqual("foo", relationship21.FirstName);
            Assert.AreEqual("blam", relationship21.LastName);
            Assert.AreEqual("baz", relationship21.Email);
            // Status bits
            Assert.AreEqual(true, relationship21.FirstNameMatches);
            Assert.AreEqual(false, relationship21.LastNameMatches);
            Assert.AreEqual(true, relationship21.EmailMatches);
            Assert.AreEqual(true, relationship21.ContactExists);
            Assert.AreEqual(Account2, relationship21.AccountEmail);

            // Column 2, contact 2
            var relationship22 = set.ContactGrid[Account2][1];
            Assert.AreEqual("zort", relationship22.FirstName);
            Assert.AreEqual("meep", relationship22.LastName);
            Assert.AreEqual("yarg", relationship22.Email);
            // Status bits
            Assert.AreEqual(true, relationship22.FirstNameMatches);
            Assert.AreEqual(true, relationship22.LastNameMatches);
            Assert.AreEqual(true, relationship22.EmailMatches);
            Assert.AreEqual(true, relationship22.ContactExists);
            Assert.AreEqual(Account2, relationship22.AccountEmail);
        }

        [TestMethod]
        public async Task MergeContactListsMultipleListEmailMismatchTest()
        {
            // Arrange
            var list1 = _contactFactory.CreateContactList(Account1, EContactAccountType.Google);
            var list2 = _contactFactory.CreateContactList(Account2, EContactAccountType.Facebook);

            var contact1 = _contactFactory.CreateContact("foo", "bar", "baz");
            var contact2 = _contactFactory.CreateContact("zort", "meep", "yarg");
            var contact3 = _contactFactory.CreateContact("foo", "bar", "blam");

            list1.Contacts.Add(contact1);
            list2.Contacts.Add(contact2);
            list2.Contacts.Add(contact3);

            var engine = new ContactMatchingEngine(_contactFactory);

            // Act
            var set = await engine.MergeContactLists(new List<ContactList> { list1, list2 });

            // Assert
            // Account Asserts, 2 columns with 2 rows
            Assert.IsNotNull(set);
            Assert.AreEqual(2, set.ContactGrid.Count);
            Assert.IsTrue(set.ContactGrid.ContainsKey(Account1));
            Assert.IsTrue(set.ContactGrid.ContainsKey(Account2));
            Assert.AreEqual(2, set.ContactGrid[Account1].Count);
            Assert.AreEqual(2, set.ContactGrid[Account2].Count);

            // Column 1, contact 1
            var relationship11 = set.ContactGrid[Account1][0];
            Assert.AreEqual("foo", relationship11.FirstName);
            Assert.AreEqual("bar", relationship11.LastName);
            Assert.AreEqual("baz", relationship11.Email);
            // Status bits
            Assert.AreEqual(true, relationship11.FirstNameMatches);
            Assert.AreEqual(true, relationship11.LastNameMatches);
            Assert.AreEqual(false, relationship11.EmailMatches);
            Assert.AreEqual(true, relationship11.ContactExists);
            Assert.AreEqual(Account1, relationship11.AccountEmail);

            // Column 1, contact 2
            var relationship12 = set.ContactGrid[Account1][1];
            Assert.AreEqual("", relationship12.FirstName);
            Assert.AreEqual("", relationship12.LastName);
            Assert.AreEqual("", relationship12.Email);
            // Status bits
            Assert.AreEqual(true, relationship12.FirstNameMatches);
            Assert.AreEqual(true, relationship12.LastNameMatches);
            Assert.AreEqual(true, relationship12.EmailMatches);
            Assert.AreEqual(false, relationship12.ContactExists);
            Assert.AreEqual(Account1, relationship12.AccountEmail);

            // Column 2, contact 1
            var relationship21 = set.ContactGrid[Account2][0];
            Assert.AreEqual("foo", relationship21.FirstName);
            Assert.AreEqual("bar", relationship21.LastName);
            Assert.AreEqual("blam", relationship21.Email);
            // Status bits
            Assert.AreEqual(true, relationship21.FirstNameMatches);
            Assert.AreEqual(true, relationship21.LastNameMatches);
            Assert.AreEqual(false, relationship21.EmailMatches);
            Assert.AreEqual(true, relationship21.ContactExists);
            Assert.AreEqual(Account2, relationship21.AccountEmail);

            // Column 2, contact 2
            var relationship22 = set.ContactGrid[Account2][1];
            Assert.AreEqual("zort", relationship22.FirstName);
            Assert.AreEqual("meep", relationship22.LastName);
            Assert.AreEqual("yarg", relationship22.Email);
            // Status bits
            Assert.AreEqual(true, relationship22.FirstNameMatches);
            Assert.AreEqual(true, relationship22.LastNameMatches);
            Assert.AreEqual(true, relationship22.EmailMatches);
            Assert.AreEqual(true, relationship22.ContactExists);
            Assert.AreEqual(Account2, relationship22.AccountEmail);
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
        public async Task ContactsMatchOnlyFirstNameMatchesTest()
        {
            // Arrange
            var contact1 = _contactFactory.CreateContact("foo", "Bar", "baz");
            var contact2 = _contactFactory.CreateContact("Foo", "meep", "yarg");
            var engine = new ContactMatchingEngine(_contactFactory);

            // Act
            var matching = await engine.ContactsMatch(contact1, contact2);

            // Assert
            Assert.IsFalse(matching);
        }

        [TestMethod]
        public async Task ContactsMatchOnlyLastNameMatchesTest()
        {
            // Arrange
            var contact1 = _contactFactory.CreateContact("foo", "Bar", "baz");
            var contact2 = _contactFactory.CreateContact("zort", "Bar", "yarg");
            var engine = new ContactMatchingEngine(_contactFactory);

            // Act
            var matching = await engine.ContactsMatch(contact1, contact2);

            // Assert
            Assert.IsFalse(matching);
        }

        [TestMethod]
        public async Task ContactsMatchOnlyEmailMatchesTest()
        {
            // Arrange
            var contact1 = _contactFactory.CreateContact("foo", "Bar", "baz");
            var contact2 = _contactFactory.CreateContact("zort", "meep", "baz");
            var engine = new ContactMatchingEngine(_contactFactory);

            // Act
            var matching = await engine.ContactsMatch(contact1, contact2);

            // Assert
            Assert.IsTrue(matching);
        }

        [TestMethod]
        public async Task ContactsMatchOnlyEmailDifferentTest()
        {
            // Arrange
            var contact1 = _contactFactory.CreateContact("foo", "Bar", "baz");
            var contact2 = _contactFactory.CreateContact("foo", "Bar", "ba");
            var engine = new ContactMatchingEngine(_contactFactory);

            // Act
            var matching = await engine.ContactsMatch(contact1, contact2);

            // Assert
            Assert.IsTrue(matching);
        }

        [TestMethod]
        public async Task ContactsMatchAllDifferentTest()
        {
            // Arrange
            var contact1 = _contactFactory.CreateContact("foo", "Bar", "baz");
            var contact2 = _contactFactory.CreateContact("zort", "meep", "yarg");
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