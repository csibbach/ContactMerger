import AccountList = require("components/AccountList/AccountList.viewModel");
import ContactAccountConnectorMock = require("tests/mocks/dataProviders/ContactAccountConnectorMock");
import AccountListParams = require("components/AccountList/AccountListParams");

QUnit.module("AccountList");

test("will return correct version from core", (assert: QUnitAssert) =>{
    // Arrange
    var connector = new ContactAccountConnectorMock(assert);
    var accountList = new AccountList(connector);

    var params = new AccountListParams();

    // Act
    accountList.setup(params);

    // Assert
    assert.ok(accountList.accountNames);
});