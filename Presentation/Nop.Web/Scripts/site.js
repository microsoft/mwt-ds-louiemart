$(function () {
    var extraSpace = $(window).width() - $('.master-wrapper-page').width();
    var defaultTraceWidth = $('#master-wrapper-trace').width();
    var traceWidth = Math.min(400, Math.max(defaultTraceWidth, extraSpace / 2));
    $('#master-wrapper-trace').width(traceWidth);

    $("#master-trace-image").resizable({
        handles: 's'
    });

    var defaultLiveTraceClass = "";
    var classes = $("#master-trace-image").attr('class').split(" ");
    for (var i = 0; i < classes.length; i++) {
        if (classes[i].indexOf("trace-") >= 0) {
            continue;
        }
        defaultLiveTraceClass += classes[i] + " ";
    }
    var defaultHoverTraceClass = $("#master-trace-hover").attr('class');

    updateTraceImage = function ($element, message, defaultClass, useGif) {
        var prefixClass = useGif ? 'trace-gif-' : 'trace-image-';

        var clientToServerInteractionClass = prefixClass + 'client-server-interaction';
        var clientToServerRewardClass = prefixClass + 'client-server-reward';
        var storageToClientClass = prefixClass + 'storage-client';
        var serverToStorageClass = prefixClass + 'server-storage';
        var azuremlToStorageClass = prefixClass + 'azureml-storage';

        var updated = true;

        if (message.toLowerCase().indexOf("model update") >= 0) {
            $element.removeClass();
            $element.addClass(defaultClass + ' ' + storageToClientClass);
        }
        else if (message.toLowerCase().indexOf("new data created") >= 0) {
            $element.removeClass();
            $element.addClass(defaultClass + ' ' + serverToStorageClass);
        }
        else if (message.toLowerCase().indexOf("requested model retraining") >= 0) {
            $element.removeClass();
            $element.addClass(defaultClass + ' ' + azuremlToStorageClass);
        }
        else if (message.toLowerCase().indexOf("successfully uploaded") >= 0) {
            $element.removeClass();
            $element.addClass(defaultClass + ' ' + clientToServerInteractionClass);
        }
        else if (message.toLowerCase().indexOf("reported rewards") >= 0) {
            $element.removeClass();
            $element.addClass(defaultClass + ' ' + clientToServerRewardClass);
        }
        else {
            updated = false;
        }

        return updated;
    };

    var momentFormat = "M/DD/YYYY h:mm:ss a";
    var chat = $.connection.traceHub;
    chat.client.addNewMessageToPage = function (message, timestamp) {
        $('#master-wrapper-trace .buttons').after(
            '<div>' +
            '<p class="master-trace-date">' + moment(timestamp).format(momentFormat) + '</p>' +
            '<p class="master-trace-message">' + message + '</p>' +
            '</div>');

        updateTraceImage($('#master-trace-image'), message, defaultLiveTraceClass, true);
    };
    $.connection.hub.start();

    $hoverDiv = $('#master-trace-hover');
    $('#master-wrapper-trace').on({
        mouseenter: function () {
            var offset = $(this).offset();

            $(this).css('background-color', 'yellow');

            var top = Math.min($(window).height() + $(window).scrollTop() - $hoverDiv.height(), Math.max(0, offset.top));
            var left = offset.left - $hoverDiv.width() - 25;
            $hoverDiv.css('top', top);
            $hoverDiv.css('left', left);

            if (updateTraceImage($hoverDiv, $(this).text(), defaultHoverTraceClass, false))
            {
                $hoverDiv.show();
            }
        },
        mouseleave: function () {
            $(this).css('background-color', 'white');
            $hoverDiv.hide();
        }
    }, '.master-trace-message');

    $('.master-trace-date').each(function () {
        var momentDateTime = moment(Number($(this).attr('data-utcdate')));
        $(this).html(momentDateTime.format(momentFormat));
    });
});