define(["require", "exports", "components/AccountList/AccountList.viewModel", "tests/mocks/dataProviders/ContactConnectorMock", "components/AccountList/AccountListParams"], function (require, exports, AccountList, ContactConnectorMock, AccountListParams) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
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
    });
});
//# sourceMappingURL=AccountList.test.js.map