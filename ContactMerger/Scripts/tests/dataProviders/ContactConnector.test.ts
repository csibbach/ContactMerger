import PromiseUtils = require("tests/utility/PromiseUtils");
import AjaxConnectorMock = require("tests/mocks/dataProviders/AjaxConnectorMock");
import ContactConnector = require("dataProviders/implementations/ContactConnector");
import Contact = require("models/Contact");

QUnit.module("ContactConnector Tests");

QUnit.test("addContacts Test", (assert: QUnitAssert) => {
    // Arrange
    var ajaxConnector = new AjaxConnectorMock(assert);

    var connector = new ContactConnector(ajaxConnector);

    var contact = new Contact();
    contact.accountEmail = "account1@test.com";
    contact.firstName = "Phoebe";
    contact.lastName = "Yuriko";
    contact.email = "phoebe@cute.com";

    // Act
    var promise = connector.addContacts(contact);

    // Assert
    PromiseUtils.assertPending(promise, () => {
        assert.equal(ajaxConnector.postUrl, "/Contact/AddContacts");
        assert.equal(ajaxConnector.postContent.AccountEmail, contact.accountEmail);
        assert.equal(ajaxConnector.postContent.FirstName, contact.firstName);
        assert.equal(ajaxConnector.postContent.LastName, contact.lastName);
        assert.equal(ajaxConnector.postContent.Email, contact.email);
    }, assert);
});

QUnit.test("getContactAccounts Test", (assert: QUnitAssert) => {
    // Arrange
    var ajaxConnector = new AjaxConnectorMock(assert);

    var connector = new ContactConnector(ajaxConnector);

    // Act
    var promise = connector.getContactAccounts();

    // Assert
    PromiseUtils.assertPending(promise, () => {
        assert.equal(ajaxConnector.getUrl, "/ContactAccount/getAccounts");
        assert.equal(promise, ajaxConnector.getControls.promise);
    }, assert);
});


QUnit.test("getContacts Test", (assert: QUnitAssert) => {
    // Arrange
    var ajaxConnector = new AjaxConnectorMock(assert);

    var connector = new ContactConnector(ajaxConnector);

    // Act
    var promise = connector.getContacts();

    // Assert
    PromiseUtils.assertPending(promise, () => {
        assert.equal(ajaxConnector.getUrl, "/Contact/GetContactSet");
        assert.equal(promise, ajaxConnector.getControls.promise);
    }, assert);
});