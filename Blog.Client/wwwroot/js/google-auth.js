window.currentComponent = null;

window.initGoogleAuth = async (clientId, dotNetRef) => {
    while (typeof google === 'undefined' || !google.accounts) {
        await new Promise(resolve => setTimeout(resolve, 100));
    }

    window.currentComponent = dotNetRef;

    google.accounts.id.initialize({
        client_id: clientId,
        callback: handleCredentialResponse
    });

  
    const container = document.getElementById('googleButtonContainer');
    if (container) {
        google.accounts.id.renderButton(container, {
            theme: 'outline',
            size: 'large',
            width: 243,
            text: 'signin_with',
            shape: 'rectangular'
        });
    }
};
function handleCredentialResponse(response) {
    if (window.currentComponent && response.credential) {
        window.currentComponent.invokeMethodAsync('HandleGoogleCallback', response.credential);
    }
}