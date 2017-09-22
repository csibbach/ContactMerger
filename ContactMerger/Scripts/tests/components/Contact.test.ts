import ko = require("knockout");
import PromiseUtils = require("tests/utility/PromiseUtils");
import ContactParams = require("components/Contact/ContactParams");
import Contact = require("models/Contact");
import ContactViewModel = require("components/Contact/Contact.viewModel");
import ContactConnectorMock = require("tests/mocks/dataProviders/ContactConnectorMock");
import ButtonContracts = require("components/Button/Button.contracts");
import EButtonType = ButtonContracts.EButtonType;

QUnit.module("Contact Tests");

QUnit.test("Basic Setup Test", (assert: QUnitAssert) => {
    // Arrange
    var contactConnector = new ContactConnectorMock(assert);

    var contact = new Contact();
    contact.firstName = "Phoebe";
    contact.lastName = "Yuriko";
    contact.email = "phoebe@cute.com";
    contact.firstNameMatches = true;
    contact.lastNameMatches = true;
    contact.emailMatches = true;
    contact.contactExists = true;
    contact.accountEmail = "account1@test.com";

    var syncWasRequested: boolean = false;
    var syncRequested = ko.observable(false);
    syncRequested.subscribe(() => {
        syncWasRequested = true;
    });
    var notOnlyColumn = ko.observable(false);

    var params = new ContactParams(ko.observable(contact), syncRequested, notOnlyColumn);

    var vm = new ContactViewModel(contactConnector);

    // Act
    vm.setup(params);

    // Assert
    assert.equal(vm.name(), "Phoebe Yuriko");
    assert.equal(vm.nameMatches(), true);
    assert.equal(vm.email(), "phoebe@cute.com");
    assert.equal(vm.emailMatches(), true);
    assert.equal(vm.contactExists(), true);
    assert.equal(vm.showErrorMessage(), false);
    assert.equal(vm.notOnlyColumn, notOnlyColumn);
    assert.equal(syncWasRequested, false);
    assert.ok(vm.syncButton);
    assert.equal(vm.syncButton.label, "Sync");
    assert.equal(vm.syncButton.buttonType, EButtonType.Primary);
    assert.ok(vm.syncButton.action);
});

QUnit.test("No Name No Email Partial Matches Test", (assert: QUnitAssert) => {
    // Arrange
    var contactConnector = new ContactConnectorMock(assert);

    var contact = new Contact();
    contact.firstName = "";
    contact.lastName = "";
    contact.email = "";
    contact.firstNameMatches = false;
    contact.lastNameMatches = true;
    contact.emailMatches = false;
    contact.contactExists = true;
    contact.accountEmail = "account1@test.com";

    var syncWasRequested: boolean = false;
    var syncRequested = ko.observable(false);
    syncRequested.subscribe(() => {
        syncWasRequested = true;
    });
    var notOnlyColumn = ko.observable(false);

    var params = new ContactParams(ko.observable(contact), syncRequested, notOnlyColumn);

    var vm = new ContactViewModel(contactConnector);

    // Act
    vm.setup(params);

    // Assert
    assert.equal(vm.name(), "No name");
    assert.equal(vm.nameMatches(), false);
    assert.equal(vm.email(), "No email");
    assert.equal(vm.emailMatches(), false);
    assert.equal(vm.contactExists(), true);
    assert.equal(vm.showErrorMessage(), false);
    assert.equal(vm.notOnlyColumn, notOnlyColumn);
    assert.equal(syncWasRequested, false); assert.ok(vm.syncButton);
    assert.equal(vm.syncButton.label, "Sync");
    assert.equal(vm.syncButton.buttonType, EButtonType.Primary);
    assert.ok(vm.syncButton.action);
});

QUnit.test("Placeholder Contact Test", (assert: QUnitAssert) => {
    // Arrange
    var contactConnector = new ContactConnectorMock(assert);

    var contact = new Contact();
    contact.firstName = "Phoebe";
    contact.lastName = "Yuriko";
    contact.email = "phoebe@cute.com";
    contact.firstNameMatches = true;
    contact.lastNameMatches = true;
    contact.emailMatches = true;
    contact.contactExists = false;
    contact.accountEmail = "account1@test.com";

    var syncWasRequested: boolean = false;
    var syncRequested = ko.observable(false);
    syncRequested.subscribe(() => {
        syncWasRequested = true;
    });
    var notOnlyColumn = ko.observable(false);

    var params = new ContactParams(ko.observable(contact), syncRequested, notOnlyColumn);

    var vm = new ContactViewModel(contactConnector);

    // Act
    vm.setup(params);

    // Assert
    assert.equal(vm.name(), "Phoebe Yuriko");
    assert.equal(vm.nameMatches(), true);
    assert.equal(vm.email(), "phoebe@cute.com");
    assert.equal(vm.emailMatches(), true);
    assert.equal(vm.contactExists(), false);
    assert.equal(vm.showErrorMessage(), false);
    assert.equal(vm.notOnlyColumn, notOnlyColumn);
    assert.equal(syncWasRequested, false);
    assert.ok(vm.syncButton);
    assert.equal(vm.syncButton.label, "Sync");
    assert.equal(vm.syncButton.buttonType, EButtonType.Primary);
    assert.ok(vm.syncButton.action);
});