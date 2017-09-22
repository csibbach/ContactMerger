define(["require", "exports", "components/AccountList/AccountList.viewModel", "tests/mocks/dataProviders/ContactConnectorMock", "components/AccountList/AccountListParams", "components/Button/Button.contracts", "tests/utility/PromiseUtils", "models/ContactAccount", "enum/EAccountType"], function (require, exports, AccountList, ContactConnectorMock, AccountListParams, ButtonContracts, PromiseUtils, ContactAccount, EAccountType) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    var EButtonType = ButtonContracts.EButtonType;
    QUnit.module("AccountList");
    QUnit.test("Basic Setup Test", function (assert) {
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
    QUnit.test("Setup Pending Test", function (assert) {
        // Arrange
        var connector = new ContactConnectorMock(assert);
        var accountList = new AccountList(connector);
        var params = new AccountListParams();
        // Act
        accountList.setup(params);
        // Assert
        PromiseUtils.assertPending(accountList.setupTestPromise, function () {
            assert.ok(accountList.accountNames);
            assert.equal(accountList.accountNames().length, 0);
            assert.equal(accountList.showErrorMessage(), false);
            assert.ok(accountList.addGoogleAccount);
            assert.equal(accountList.addGoogleAccount.label, "Add Google Account");
            assert.equal(accountList.addGoogleAccount.buttonType, EButtonType.Default);
            assert.ok(accountList.addGoogleAccount.action);
        }, assert);
    });
    QUnit.test("Setup Complete Test", function (assert) {
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
        PromiseUtils.assertResolved(accountList.setupTestPromise, function () {
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
        }, assert);
    });
    QUnit.test("Setup Failed Test", function (assert) {
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
        PromiseUtils.assertResolved(accountList.setupTestPromise, function () {
            assert.ok(accountList.accountNames);
            assert.equal(accountList.accountNames().length, 0);
            assert.equal(accountList.showErrorMessage(), true);
            assert.ok(accountList.addGoogleAccount);
            assert.equal(accountList.addGoogleAccount.label, "Add Google Account");
            assert.equal(accountList.addGoogleAccount.buttonType, EButtonType.Default);
            assert.ok(accountList.addGoogleAccount.action);
        }, assert);
    });
});
//# sourceMappingURL=AccountList.test.js.map