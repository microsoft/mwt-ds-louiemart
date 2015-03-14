$(function () {
    var extraSpace = $(window).width() - $('.master-wrapper-page').width();
    var defaultTraceWidth = $('#master-wrapper-trace').width();
    var traceWidth = Math.min(500, Math.max(defaultTraceWidth, extraSpace / 2));
    $('#master-wrapper-trace').width(traceWidth);

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

    $("#master-trace-image").resizable({
        handles: 's'
    });

    $('.master-trace-date').each(function () {
        var momentDateTime = moment(Number($(this).attr('data-utcdate')));
        $(this).html(momentDateTime.format(momentFormat));
    });
});