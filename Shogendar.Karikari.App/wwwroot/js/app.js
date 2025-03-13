export function addResizeListener(dotNetHelper) {
    window.addEventListener('resize', () => {
        dotNetHelper.invokeMethodAsync('OnBrowserResize');
    });
}

export function removeResizeListener(dotNetHelper) {
    window.removeEventListener('resize', () => {
        dotNetHelper.invokeMethodAsync('OnBrowserResize');
    });
}
