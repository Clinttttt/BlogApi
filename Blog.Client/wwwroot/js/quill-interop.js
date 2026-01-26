window.initQuill = (element, initialContent, toolbar) => {
    if (typeof Quill === 'undefined') {
        console.error('Quill is not defined!');
        return null;
    }

    const toolbarOptions = JSON.parse(toolbar);
    const quill = new Quill(element, {
        theme: 'snow',
        placeholder: 'Start writing your content here...',
        modules: {
            toolbar: toolbarOptions,
            syntax: {
                hljs: window.hljs  
            }
        }
    });

    if (initialContent) {
        quill.root.innerHTML = initialContent;
    }

    return quill;
};

window.getQuillContent = (quill) => {
    return quill.root.innerHTML;
};

window.setQuillContent = (quill, html) => {
    quill.root.innerHTML = html;
};

window.injectQuillStyles = (styles) => {
    const styleElement = document.getElementById('quill-custom-styles');
    if (styleElement) {
        styleElement.textContent = styles;
    }
};