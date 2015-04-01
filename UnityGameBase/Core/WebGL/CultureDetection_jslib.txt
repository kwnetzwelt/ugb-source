var CultureDetectionPlugin = {

    DetectBrowserLanguage: function () {

        // detects if a user has preferred languages
        // supported by Chrome & Firefox > 32 (HTML5.1 Feature)
        function IsPreferredSupported() {
            return (typeof navigator.languages != 'undefined' && navigator.languages.length > 0);
        }

        function GetLanguage() {
            var language = navigator.language;
            if (typeof language == 'undefined')
            {
                // at this point we have an IE user
                language = navigator.systemLanguage;
            }
            return language;
        }
        

        // main
        var langResult = (IsPreferredSupported()) ? navigator.languages[0] : GetLanguage();
        var resultPtr = _malloc(langResult.length + 4);
        writeStringToMemory(langResult, resultPtr);
        return resultPtr;
    }
};

mergeInto(LibraryManager.library, CultureDetectionPlugin);