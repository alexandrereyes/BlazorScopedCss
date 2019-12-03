window.blazorScopedCss = {
    innerHTML: function (elementId, value) {
        document.getElementById(elementId).innerHTML = value;
    }
};