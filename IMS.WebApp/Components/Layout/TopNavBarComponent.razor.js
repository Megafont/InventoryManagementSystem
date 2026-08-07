let clickHandler = null;
let dotNetRef = null;

export function setupOutsideClickListener(dotNetObjRef) {
    // Clean up any existing listener first
    cleanupOutsideClickListener();

    dotNetRef = dotNetObjRef;

    clickHandler = (event) => {
        const navbar = document.querySelector('nav.navbar');
        if (navbar && !navbar.contains(event.target)) {
            dotNetRef.invokeMethodAsync('CloseDropdownsFromOutside');
        }
    };

    document.addEventListener('click', clickHandler);
}

export function cleanupOutsideClickListener() {
    if (clickHandler) {
        document.removeEventListener('click', clickHandler);
        clickHandler = null;
    }
    dotNetRef = null;
}