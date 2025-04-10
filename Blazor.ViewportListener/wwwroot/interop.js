export const ViewportListener = {
    listeners: [],

    addListener: function (dotNetComponent, callbackMethodName) {
        this.listeners.push({ dotNetComponent: dotNetComponent, callbackMethodName: callbackMethodName });

        if (this.listeners.length === 1) {
            window.addEventListener("resize", this.handleResize.bind(this));
        }
    },

    removeListener: function (dotNetComponent) {
        this.listeners = this.listeners.filter(listener => listener.component !== dotNetComponent);

        if (this.listeners.length === 0) {
            window.removeEventListener("resize", this.handleResize.bind(this));
        }
    },

    handleResize: function () {
        this.listeners.forEach(listener => {
            listener.dotNetComponent.invokeMethodAsync(listener.callbackMethodName);
        });
    },

    getViewportInfo: function () {
        return { width: window.innerWidth, height: window.innerHeight };
    }
};