export const ViewportListener = {
    listeners: [],

    addListener: function (component, callbackMethodName) {
        // Store the reference to the .NET method and the DotNetObjectReference
        this.listeners.push({ component: component, methodName: callbackMethodName });

        if (this.listeners.length === 1) {
            // Add the global resize listener
            window.addEventListener("resize", this.handleResize.bind(this));
        }
    },

    removeListener: function (component) {
        // Find and remove the listener
        this.listeners = this.listeners.filter(listener => listener.component !== component);

        if (this.listeners.length === 0) {
            // Remove the global resize listener when no listeners remain
            window.removeEventListener("resize", this.handleResize.bind(this));
        }
    },

    handleResize: function () {
        // Notify all listeners
        this.listeners.forEach(listener => {
            listener.component.invokeMethodAsync(listener.methodName);
        });
    },

    getViewportInfo: function () {
        return { width: window.innerWidth, height: window.innerHeight };
    }
};