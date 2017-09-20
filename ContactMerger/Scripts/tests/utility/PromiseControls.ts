class PromiseControls<T> {
    public promise: Promise<T>;

    protected resolveState: any = null;
    protected resolveValue: T;
    protected rejectValue: Error;
    protected resolveFunction: (resolution: T) => void;
    protected rejectFunction: (error: Error) => void;
    protected done: () => void;

    constructor(protected assert: QUnitAssert) {
        this.promise = new Promise<T>((resolve: (resolution: T) => void, reject: (error: Error) => void) => {
            this.resolveFunction = resolve;
            this.rejectFunction = reject;
            if (this.resolveState == null) {
            } else if (this.resolveState) {
                resolve(this.resolveValue);
                this.done();
            } else {
                reject(this.rejectValue);
                this.done();
            }
        });
    }

    public resolve(resolution: T = null) {
        this.done = this.assert.async();
        if (this.resolveFunction != null) {
            this.resolveFunction(resolution);
            this.done();
        } else {
            this.resolveState = true;
            this.resolveValue = resolution;
        }

    }

    public reject(error: Error = new Error()) {
        this.done = this.assert.async();
        if (this.rejectFunction != null) {
            this.rejectFunction(error);
            this.done();
        } else {
            this.resolveState = false;
            this.rejectValue = error;
        }
    }

}

export = PromiseControls;