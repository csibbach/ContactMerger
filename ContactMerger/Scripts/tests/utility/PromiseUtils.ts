class PromiseUtils {
    public static afterResolved<T>(assert: QUnitAssert, promise: Promise<T>, thenFunction: (params: T) => void) {
        var done = assert.async();

        promise.then((thenParams: T) => {
            thenFunction(thenParams);
            done();
        });
    }

    public static assertPending<T>(promise: Promise<T>, pendingFunction: () => void, assert: QUnitAssert, assertMessage: string = null, pendingTime: number = 100) {
        var done = assert.async();
        var failed = false;
        promise.then(() => {
            assert.equal("resolved", "pending", assertMessage);
            failed = true;
            done();
        }, () => {
            failed = true;
            assert.equal("rejected", "pending", assertMessage);
            done();
        });

        setTimeout(() => {
            if (!failed) {
                assert.equal("pending", "pending", assertMessage);
                pendingFunction();
                done();
            }
        }, pendingTime);
    }

    public static assertResolved<T>(promise: Promise<T>, thenFunction: (params: T) => void, assert: QUnitAssert, assertMessage: string = null, pendingTime: number = 100) {
        var done = assert.async();
        var handled = false;
        promise.then((thenParams: T) => {
            handled = true;
            assert.equal("resolved", "resolved", assertMessage);
            try {
                thenFunction(thenParams);
            } catch (error) {
                assert.equal("then function runs", "exception in then function", "Then callback for assertResolved has an error!!. \n" + error);
            }

            done();
            
        }, () => {
            handled = true;
            assert.equal("rejected", "resolved", assertMessage);
            done();
        });

        setTimeout(() => {
            if (!handled) {
                assert.equal("pending", "resolved", assertMessage);
                done();
            }
        }, pendingTime);
    }

    public static assertRejected<T>(promise: Promise<T>, catchFunction: (error: Error) => void, assert: QUnitAssert, assertMessage: string = null, pendingTime: number = 100) {
        var done = assert.async();
        var handled = false;
        promise.then(() => {
            handled = true;
            assert.equal("resolved", "rejected", assertMessage);
            done();
        }, (catchError: Error) => {
            handled = true;
            assert.equal("rejected", "rejected", assertMessage);
            catchFunction(catchError);
            done();
            });

        setTimeout(() => {
            if (!handled) {
                assert.equal("pending", "rejected", assertMessage);
                done();
            }
        }, pendingTime);
    }

    public static asyncAction(assert: QUnitAssert): Action {
        var firstAction = new Action(assert);
        firstAction.activate();
        return firstAction;
    }
}

class Action {
    private callback: () => void;
    private next: Action;
    private done: () => void;
    public act(callback: () => void): Action {
        this.done = this.assert.async();
        this.next = new Action(this.assert);
        this.callback = callback;

        return this.next;
    }

    public activate() {
        setTimeout(() => {
            if (this.callback != null) {
                this.callback();
                this.next.activate();
                this.done();
            }
        }, 1);
    }

    constructor(protected assert: QUnitAssert) {}
}

export = PromiseUtils;