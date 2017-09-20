define(["require", "exports", "components/AccountList/AccountList.viewModel", "tests/mocks/dataProviders/ContactAccountConnectorMock", "components/AccountList/AccountListParams"], function (require, exports, AccountList, ContactAccountConnectorMock, AccountListParams) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    QUnit.module("AccountList");
    test("will return correct version from core", function (assert) {
        // Arrange
        var connector = new ContactAccountConnectorMock(assert);
        var accountList = new AccountList(connector);
        var params = new AccountListParams();
        // Act
        accountList.setup(params);
        // Assert
        assert.ok(accountList.accountNames);
    });
});
//# sourceMappingURL=AccountList.test.js.map