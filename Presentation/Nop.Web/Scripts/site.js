$(function () {
    var extraSpace = $(window).width() - $('.master-wrapper-page').width();
    var defaultTraceWidth = $('#master-wrapper-trace').width();
    var traceWidth = Math.min(400, Math.max(defaultTraceWidth, extraSpace / 2));
    $('#master-wrapper-trace').width(traceWidth);

    $("#master-trace-image").resizable({
        handles: 's'
    });

    var defaultTraceClass = "";
    var classes = $("#master-trace-image").attr('class').split(" ");
    for (var i = 0; i < classes.length; i++) {
        if (classes[i].indexOf("trace-") >= 0) {
            continue;
        }
        defaultTraceClass += classes[i] + " ";
    }

    var momentFormat = "M/DD/YYYY h:mm:ss a";
    var chat = $.connection.traceHub;
    chat.client.addNewMessageToPage = function (message, timestamp) {
        $('#master-wrapper-trace .buttons').after(
            '<div>' +
            '<p class="master-trace-date">' + moment(timestamp).format(momentFormat) + '</p>' +
            '<p class="master-trace-message">' + htmlEncode(message) + '</p>' +
            '</div>');

        if (message.toLowerCase().indexOf("model update") >= 0) {
            $('#master-trace-image').removeClass();
            $('#master-trace-image').addClass(defaultTraceClass + ' trace-gif-storage-client');
        }
        else if (message.toLowerCase().indexOf("new data created") >= 0) {
            $('#master-trace-image').removeClass();
            $('#master-trace-image').addClass(defaultTraceClass + ' trace-gif-server-storage');
        }
        else if (message.toLowerCase().indexOf("retrain model success") >= 0) {
            $('#master-trace-image').removeClass();
            $('#master-trace-image').addClass(defaultTraceClass + ' trace-gif-azureml-storage');
        }
    };
    $.connection.hub.start();

    $('.master-trace-date').each(function () {
        var momentDateTime = moment(Number($(this).attr('data-utcdate')));
        $(this).html(momentDateTime.format(momentFormat));
    });
});