export function getDisplayName(WrappedComponent) {
    return WrappedComponent.displayName || WrappedComponent.name || 'Component';
}

export function compose(...funcs) {
    return funcs.reduce((a, b) => (...args) => a(b(...args)), arg => arg);
}

