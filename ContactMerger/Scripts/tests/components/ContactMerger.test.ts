import ko = require("knockout");
import PromiseUtils = require("tests/utility/PromiseUtils");
import ContactMergerViewModel = require("components/ContactMerger/ContactMerger.viewModel");
import ContactConnectorMock = require("tests/mocks/dataProviders/ContactConnectorMock");
import ButtonContracts = require("components/Button/Button.contracts");
import EButtonType = ButtonContracts.EButtonType;
import ContactSet = require("models/ContactSet");

QUnit.module("ContactMerger Tests");

QUnit.test("Basic Setup Test", (assert: QUnitAssert) => {
    // Arrange
    var contactConnector = new ContactConnectorMock(assert);

    var vm = new ContactMergerViewModel(contactConnector);

    // Act
    vm.setup({});

    // Assert
    assert.ok(vm.accountList);
    assert.equal(vm.showContactList(), false);
    assert.ok(vm.contactList);
    assert.equal(vm.contactList.contactSet(), null);
    assert.equal(vm.contactList.syncRequested(), false);
    assert.ok(vm.fetchContacts);
    assert.equal(vm.fetchContacts.label, "Fetch Contacts");
    assert.equal(vm.fetchContacts.buttonType, EButtonType.Primary);
    assert.ok(vm.fetchContacts.action);
    assert.equal(vm.showErrorMessage(), false);
});

QUnit.test("Sync Requested Pending Test", (assert: QUnitAssert) => {
    // Arrange
    var contactConnector = new ContactConnectorMock(assert);

    var vm = new ContactMergerViewModel(contactConnector);

    // Act
    vm.setup({});
    vm.contactList.syncRequested.valueHasMutated();

    // Assert
    PromiseUtils.assertPending(vm.updateContactSetPromise, () => {
        assert.ok(vm.accountList);
        assert.equal(vm.showContactList(), false);
        assert.ok(vm.contactList);
        assert.equal(vm.contactList.contactSet(), null);
        assert.equal(vm.contactList.syncRequested(), false);
        assert.ok(vm.fetchContacts);
        assert.equal(vm.fetchContacts.label, "Fetch Contacts");
        assert.equal(vm.fetchContacts.buttonType, EButtonType.Primary);
        assert.ok(vm.fetchContacts.action);
        assert.equal(vm.showErrorMessage(), false);
    }, assert);
});


QUnit.test("Sync Requested Complete Test", (assert: QUnitAssert) => {
    // Arrange
    var contactConnector = new ContactConnectorMock(assert);
    var contactSet = new ContactSet();

    var vm = new ContactMergerViewModel(contactConnector);

    // Act
    vm.setup({});
    vm.contactList.syncRequested.valueHasMutated();
    contactConnector.getContactsControls.resolve(contactSet);

    // Assert
    PromiseUtils.assertResolved(vm.updateContactSetPromise, () => {
        assert.ok(vm.accountList);
        assert.equal(vm.showContactList(), true);
        assert.ok(vm.contactList);
        assert.equal(vm.contactList.contactSet(), contactSet);
        assert.equal(vm.contactList.syncRequested(), false);
        assert.ok(vm.fetchContacts);
        assert.equal(vm.fetchContacts.label, "Fetch Contacts");
        assert.equal(vm.fetchContacts.buttonType, EButtonType.Primary);
        assert.ok(vm.fetchContacts.action);
        assert.equal(vm.showErrorMessage(), false);
    }, assert);
});


QUnit.test("Sync Requested Failed Test", (assert: QUnitAssert) => {
    // Arrange
    var contactConnector = new ContactConnectorMock(assert);

    var vm = new ContactMergerViewModel(contactConnector);

    // Act
    vm.setup({});
    vm.contactList.syncRequested.valueHasMutated();
    contactConnector.getContactsControls.reject();

    // Assert
    PromiseUtils.assertRejected(vm.updateContactSetPromise, () => {
        assert.ok(vm.accountList);
        assert.equal(vm.showContactList(), false);
        assert.ok(vm.contactList);
        assert.equal(vm.contactList.contactSet(), null);
        assert.equal(vm.contactList.syncRequested(), false);
        assert.ok(vm.fetchContacts);
        assert.equal(vm.fetchContacts.label, "Fetch Contacts");
        assert.equal(vm.fetchContacts.buttonType, EButtonType.Primary);
        assert.ok(vm.fetchContacts.action);
        assert.equal(vm.showErrorMessage(), true);
    }, assert);

});