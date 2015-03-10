$(function () {
    var chat = $.connection.traceHub;
    chat.client.addNewMessageToPage = function (message) {
        $('#master-wrapper-trace').append('<div><p class="master-trace-message">' + htmlEncode(message) + '</p></div>');
    };
    $.connection.hub.start();
});