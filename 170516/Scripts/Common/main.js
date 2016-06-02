var Util = {
    GetBase64Url: function (fileType, source) {
        return "data:image/" + fileType + ";base64, " + source;
    },
    IsNullOrWhiteSpace: function (raw) {
        if (raw == null || raw == undefined) return true;

        if (raw.trim() == "") return true

        return false;
    }
}


// PLUGIN
$.fn.IsNullOrWhiteSpace = function (raw) {
    if (raw == null || raw == undefined) return true;

    if (raw.trim() == "") return true

    return false;
}