import ko = require("knockout");
import ContactListViewModel = require("components/ContactList/ContactList.viewModel");
import ContactListParams = require("components/ContactList/ContactListParams");
import ContactSet = require("models/ContactSet");
import Contact = require("models/Contact");

QUnit.module("ContactList Tests");

QUnit.test("Basic Setup Test", (assert: QUnitAssert) => {
    // Arrange
    var contact1 = createContact(1, "account1@test.com");
    var contactSet = new ContactSet();
    contactSet.contactGrid = {
        "account1@test.com": [contact1]
    };

    var contactSetObservable = ko.observable(contactSet);
    var syncRequested = ko.observable(false);
    var params = new ContactListParams(contactSetObservable, syncRequested);

    var vm = new ContactListViewModel();

    // Act
    vm.setup(params);

    // Assert
    assert.equal(vm.accountNames().length, 1);
    assert.equal(vm.accountNames()[0], "account1@test.com");
    assert.equal(vm.contactGrid().length, 1);
    assert.equal(vm.contactGrid()[0].length, 1);
    assert.ok(vm.contactGrid()[0][0]);
    assert.equal(vm.contactGrid()[0][0].syncRequested, syncRequested);
    assert.equal(vm.contactGrid()[0][0].contact(), contact1);
    assert.equal(vm.contactGrid()[0][0].notOnlyColumn(), false);
});

QUnit.test("Simple to Complex Test",
    (assert: QUnitAssert) => {
        // Arrange
        var contact1 = createContact(1, "account1@test.com");
        var contact2 = createContact(2, "account1@test.com");
        var contact3 = createContact(3, "account2@test.com");
        var contact4 = createContact(4, "account2@test.com", false);

        var contactSet1 = new ContactSet();
        contactSet1.contactGrid = {
            "account1@test.com": [contact1]
        };

        var contactSet2 = new ContactSet();
        contactSet2.contactGrid = {
            "account1@test.com": [contact1, contact2],
            "account2@test.com": [contact3, contact4]
        };

        var contactSetObservable = ko.observable(contactSet1);
        var syncRequested = ko.observable(false);
        var params = new ContactListParams(contactSetObservable, syncRequested);

        var vm = new ContactListViewModel();

        // Act
        vm.setup(params);
        contactSetObservable(contactSet2);
        
        // Assert
       
        assert.equal(vm.accountNames().length, 2);
        assert.equal(vm.accountNames()[0], "account1@test.com");
        assert.equal(vm.accountNames()[1], "account2@test.com");
        assert.equal(vm.contactGrid().length, 2);
        assert.equal(vm.contactGrid()[0].length, 2);
        assert.equal(vm.contactGrid()[1].length, 2);

        assert.ok(vm.contactGrid()[0][0]);
        assert.equal(vm.contactGrid()[0][0].syncRequested, syncRequested);
        assert.equal(vm.contactGrid()[0][0].contact(), contact1);
        assert.equal(vm.contactGrid()[0][0].notOnlyColumn(), true);

        assert.ok(vm.contactGrid()[0][1]);
        assert.equal(vm.contactGrid()[0][1].syncRequested, syncRequested);
        assert.equal(vm.contactGrid()[0][1].contact(), contact3);
        assert.equal(vm.contactGrid()[0][1].notOnlyColumn(), true);

        assert.ok(vm.contactGrid()[1][0]);
        assert.equal(vm.contactGrid()[1][0].syncRequested, syncRequested);
        assert.equal(vm.contactGrid()[1][0].contact(), contact2);
        assert.equal(vm.contactGrid()[1][0].notOnlyColumn(), true);

        assert.ok(vm.contactGrid()[1][1]);
        assert.equal(vm.contactGrid()[1][1].syncRequested, syncRequested);
        assert.equal(vm.contactGrid()[1][1].contact(), contact4);
        assert.equal(vm.contactGrid()[1][1].notOnlyColumn(), true);
});

function createContact(index: number, account: string, contactExists: boolean = true): Contact {
    var contact = new Contact();
    contact.firstName = "Phoebe" + index;
    contact.lastName = "Yuriko" + index;
    contact.email = "phoebe@cute.com" + index;
    contact.firstNameMatches = true;
    contact.lastNameMatches = true;
    contact.emailMatches = true;
    contact.contactExists = contactExists;
    contact.accountEmail = account;

    return contact;
}