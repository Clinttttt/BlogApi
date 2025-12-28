window.currentComponent = null;

window.initGoogleAuthWithButton = async (clientId, dotNetRef) => {
    console.log('initGoogleAuthWithButton called');

    while (typeof google === 'undefined' || !google.accounts) {
        await new Promise(resolve => setTimeout(resolve, 100));
    }

    console.log('Google API loaded');
    window.currentComponent = dotNetRef;

    google.accounts.id.initialize({
        client_id: clientId,
        callback: handleGoogleResponse
    });

    console.log('Google initialized');

    const buttonDiv = document.getElementById('googleButtonDiv');
    if (buttonDiv) {
        console.log('Rendering button');
        google.accounts.id.renderButton(
            buttonDiv,
            {
                theme: 'outline',
                size: 'large',
                width: '100%',
                text: 'signin_with',
                shape: 'rectangular'
            }
        );
    } else {
        console.error('googleButtonDiv not found');
    }
};

function handleGoogleResponse(response) {
    console.log('Google response received');
    if (window.currentComponent) {
        window.currentComponent.invokeMethodAsync('HandleGoogleLoginCallback', response.credential);
    }
}

window.triggerGoogleLogin = () => {
    if (typeof google !== 'undefined' && google.accounts) {
        google.accounts.id.prompt((notification) => {
            console.log('Notification:', notification);
            if (notification.isNotDisplayed()) {
                console.log('Reason not displayed:', notification.getNotDisplayedReason());
            }
        });
    } else {
        console.error('Google API not loaded yet');
    }
};