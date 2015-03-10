$(function () {
    var chat = $.connection.traceHub;
    chat.client.addNewMessageToPage = function (message) {
        alert(message);
        //$('#messages').append('<li><strong>' + htmlEncode(message)
        //    + '</strong></li>');
    };
    $.connection.hub.start();
});