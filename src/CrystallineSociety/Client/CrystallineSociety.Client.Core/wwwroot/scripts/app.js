"use strict";
var App = /** @class */ (function () {
    function App() {
    }
    App.applyBodyElementClasses = function (cssClasses, cssVariables) {
        cssClasses === null || cssClasses === void 0 ? void 0 : cssClasses.forEach(function (c) { return document.body.classList.add(c); });
        Object.keys(cssVariables).forEach(function (key) { return document.body.style.setProperty(key, cssVariables[key]); });
    };
    App.goBack = function () {
        window.history.back();
    };
    App.removeParametersOfUrl = function (url) {
        window.history.pushState(null, '', url);
    };
    return App;
}());
;
BitTheme.init({
    system: true,
    onChange: function (newTheme, oldThem) {
        if (newTheme === 'dark') {
            document.body.classList.add('theme-dark');
            document.body.classList.remove('theme-light');
            document.querySelector("meta[name=theme-color]").setAttribute('content', '#0d1117');
        }
        else {
            document.body.classList.add('theme-light');
            document.body.classList.remove('theme-dark');
            document.querySelector("meta[name=theme-color]").setAttribute('content', '#ffffff');
        }
    }
});
