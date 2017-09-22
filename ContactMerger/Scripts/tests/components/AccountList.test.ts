import AccountList = require("components/AccountList/AccountList.viewModel");
import ContactConnectorMock = require("tests/mocks/dataProviders/ContactConnectorMock");
import AccountListParams = require("components/AccountList/AccountListParams");
import ButtonContracts = require("components/Button/Button.contracts");
import EButtonType = ButtonContracts.EButtonType;
import PromiseUtils = require("tests/utility/PromiseUtils");
import ContactAccount = require("models/ContactAccount");
import EAccountType = require("enum/EAccountType");

QUnit.module("AccountList");

QUnit.test("Basic Setup Test", (assert: QUnitAssert) =>{
    // Arrange
    var connector = new ContactConnectorMock(assert);
    var accountList = new AccountList(connector);

    var params = new AccountListParams();

    // Act
    accountList.setup(params);

    // Assert
    assert.ok(accountList.accountNames);
    assert.equal(accountList.accountNames().length, 0);
    assert.equal(accountList.showErrorMessage(), false);
    assert.ok(accountList.addGoogleAccount);
    assert.equal(accountList.addGoogleAccount.label, "Add Google Account");
    assert.equal(accountList.addGoogleAccount.buttonType, EButtonType.Default);
    assert.ok(accountList.addGoogleAccount.action); // This is tested in another test.
});

QUnit.test("Setup Pending Test", (assert: QUnitAssert) => {
    // Arrange
    var connector = new ContactConnectorMock(assert);
    var accountList = new AccountList(connector);

    var params = new AccountListParams();

    // Act
    accountList.setup(params);

    // Assert
    PromiseUtils.assertPending(accountList.setupTestPromise,
        () => {
            assert.ok(accountList.accountNames);
            assert.equal(accountList.accountNames().length, 0);
            assert.equal(accountList.showErrorMessage(), false);
            assert.ok(accountList.addGoogleAccount);
            assert.equal(accountList.addGoogleAccount.label, "Add Google Account");
            assert.equal(accountList.addGoogleAccount.buttonType, EButtonType.Default);
            assert.ok(accountList.addGoogleAccount.action);
        },
        assert);
});

QUnit.test("Setup Complete Test", (assert: QUnitAssert) => {
    // Arrange
    var connector = new ContactConnectorMock(assert);
    var accountList = new AccountList(connector);
    var contactAccount = new ContactAccount();
    contactAccount.accountEmail = "test";
    contactAccount.contactAccountType = EAccountType.Facebook;

    var params = new AccountListParams();

    // Act
    accountList.setup(params);
    connector.getContactAccountsControls.resolve([contactAccount]);

    // Assert
    PromiseUtils.assertResolved(accountList.setupTestPromise,
        () => {
            assert.ok(accountList.accountNames);
            assert.equal(accountList.accountNames().length, 1);
            assert.equal(accountList.accountNames()[0].accountName, "test");
            assert.equal(accountList.accountNames()[0].showGoogleIcon(), false);
            assert.equal(accountList.accountNames()[0].showFacebookIcon(), true);
            assert.equal(accountList.showErrorMessage(), false);
            assert.ok(accountList.addGoogleAccount);
            assert.equal(accountList.addGoogleAccount.label, "Add Google Account");
            assert.equal(accountList.addGoogleAccount.buttonType, EButtonType.Default);
            assert.ok(accountList.addGoogleAccount.action);
        },
        assert);
});

QUnit.test("Setup Failed Test", (assert: QUnitAssert) => {
    // Arrange
    var connector = new ContactConnectorMock(assert);
    var accountList = new AccountList(connector);
    var contactAccount = new ContactAccount();
    contactAccount.accountEmail = "test";
    contactAccount.contactAccountType = EAccountType.Facebook;

    var params = new AccountListParams();

    // Act
    accountList.setup(params);
    connector.getContactAccountsControls.reject();

    // Assert
    PromiseUtils.assertResolved(accountList.setupTestPromise,
        () => {
            assert.ok(accountList.accountNames);
            assert.equal(accountList.accountNames().length, 0);
            assert.equal(accountList.showErrorMessage(), true);
            assert.ok(accountList.addGoogleAccount);
            assert.equal(accountList.addGoogleAccount.label, "Add Google Account");
            assert.equal(accountList.addGoogleAccount.buttonType, EButtonType.Default);
            assert.ok(accountList.addGoogleAccount.action);
        },
        assert);
});