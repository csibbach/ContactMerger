import Button = require("components/Button/Button.viewModel");
import ButtonContracts = require("components/Button/Button.contracts");
import ButtonParams = ButtonContracts.ButtonParams;
import EButtonType = ButtonContracts.EButtonType;
import PromiseUtils = require("tests/utility/PromiseUtils");

QUnit.module("Button Tests");

QUnit.test("Basic Setup Test", (assert: QUnitAssert) => {
    // Arrange
    var button = new Button();
    var label = "test";
    var actionClicked: boolean = false;
    var action = (): Promise<void> => {
        actionClicked = true;
        return null;
    };

    var params = new ButtonParams(label, action, EButtonType.Default);

    // Act
    button.setup(params);

    // Assert
    assert.ok(button);
    assert.equal(button.buttonLabel, label);
    assert.equal(button.defaultType, true);
    assert.equal(button.primaryType, false);
    assert.equal(button.showLoading(), false);
    assert.equal(actionClicked, false);
});

QUnit.test("Non Async Click Test", (assert: QUnitAssert) => {
    // Arrange
    var button = new Button();
    var label = "test";
    var actionClicked: boolean = false;
    var action = (): Promise<void> => {
        actionClicked = true;
        return null;
    };

    var params = new ButtonParams(label, action, EButtonType.Primary);

    // Act
    button.setup(params);
    button.buttonClicked();

    // Assert
    assert.ok(button);
    assert.equal(button.buttonLabel, label);
    assert.equal(button.defaultType, false);
    assert.equal(button.primaryType, true);
    assert.equal(button.showLoading(), false);
    assert.equal(actionClicked, true);
});

QUnit.test("Async Click Waiting Test", (assert: QUnitAssert) => {
    // Arrange
    var button = new Button();
    var label = "test";
    var actionClicked: boolean = false;
    var resolveAction: () => void;
    var action = (): Promise<boolean> => {
        actionClicked = true;

        return new Promise<boolean>((resolve: () => void, reject: () => void) => {
            resolveAction = resolve;
            return true;
        });
    };

    var params = new ButtonParams(label, action, EButtonType.Primary);

    // Act
    button.setup(params);
    var promise = button.buttonClicked();

    // Assert
    PromiseUtils.assertPending(promise,
        () => {
            assert.ok(button);
            assert.equal(button.buttonLabel, label);
            assert.equal(button.defaultType, false);
            assert.equal(button.primaryType, true);
            assert.equal(button.showLoading(), true);
            assert.equal(actionClicked, true);
        },
        assert);
});

QUnit.test("Async Click Complete Test", (assert: QUnitAssert) => {
    // Arrange
    var button = new Button();
    var label = "test";
    var actionClicked: boolean = false;
    var resolveAction: () => void;
    var action = (): Promise<boolean> => {
        actionClicked = true;

        return new Promise<boolean>((resolve: () => void, reject: () => void) => {
            resolveAction = resolve;
            return true;
        });
    };

    var params = new ButtonParams(label, action, EButtonType.Primary);

    // Act
    button.setup(params);
    var promise = button.buttonClicked();
    resolveAction();

    // Assert
    PromiseUtils.assertResolved(promise,
        () => {
            assert.ok(button);
            assert.equal(button.buttonLabel, label);
            assert.equal(button.defaultType, false);
            assert.equal(button.primaryType, true);
            assert.equal(button.showLoading(), false);
            assert.equal(actionClicked, true);
        },
        assert);
});