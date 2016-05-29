var Util = {
    getBase64Url: function (fileType, source) {
        return "data:image/" + fileType + ";base64, " + source;
    }
}