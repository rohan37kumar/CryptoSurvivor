mergeInto(LibraryManager.library, {
    SaveToIndexedDBJS: function(data) {
        var dataStr = UTF8ToString(data);
        localStorage.setItem('gameData', dataStr);
    },

    LoadFromIndexedDBJS: function() {
        var data = localStorage.getItem('gameData');
        if (data === null) {
            return "";
        }
        var bufferSize = lengthBytesUTF8(data) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(data, buffer, bufferSize);
        return buffer;
    }
});
