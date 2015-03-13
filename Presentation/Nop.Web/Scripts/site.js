$(function () {
    var momentFormat = "M/DD/YYYY h:mm:ss a";
    var chat = $.connection.traceHub;
    chat.client.addNewMessageToPage = function (message, timestamp) {
        $('#master-wrapper-trace .buttons').after(
            '<div>' +
            '<p class="master-trace-date">' + moment(timestamp).format(momentFormat) + '</p>' +
            '<p class="master-trace-message">' + htmlEncode(message) + '</p>' +
            '</div>');
    };
    $.connection.hub.start();

    $('.master-trace-date').each(function () {
        var momentDateTime = moment(Number($(this).attr('data-utcdate')));
        $(this).html(momentDateTime.format(momentFormat));
    });
});