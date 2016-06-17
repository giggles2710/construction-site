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
$.fn.isNullOrWhiteSpace = function (raw) {
    if (raw == null || raw == undefined) return true;

    if (raw.trim() == "") return true

    return false;
}

$.fn.serializeObject = function () {
    var o = {};
    var a = this.serializeArray();
    $.each(a, function () {
        if (o[this.name] !== undefined) {
            if (!o[this.name].push) {
                o[this.name] = [o[this.name]];
            }
            o[this.name].push(this.value || '');
        } else {
            o[this.name] = this.value || '';
        }
    });
    return o;
};