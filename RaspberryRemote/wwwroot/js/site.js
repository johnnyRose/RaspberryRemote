
$(function () {
    "use strict";

    var connection = new signalR.HubConnectionBuilder().withUrl("/LircHub").build();

    connection.onclose(startConnection);

    connection.on("InfaredHandler", function (button) {
        $('#feedback').append("<li>" + new Date().toLocaleString() + " - " + button + "</li>");
    });

    $('.lirc-button').on('click', function () {
        connection.invoke("ButtonPressed", $(this).val());
    });

    startConnection();

    function startConnection() {
        connection.start().then(function () {
            $('.loading').addClass('hidden');
            $('#lirc-buttons').removeClass('hidden');
        });
    }

});
