import AccountList = require("components/AccountList/AccountList.viewModel");
import ContactConnectorMock = require("tests/mocks/dataProviders/ContactConnectorMock");
import AccountListParams = require("components/AccountList/AccountListParams");

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
});