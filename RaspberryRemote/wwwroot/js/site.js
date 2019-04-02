
$(function () {
    "use strict";

    var connection = new signalR.HubConnectionBuilder().withUrl("/LircHub").build();

    connection.on("InfaredHandler", function (button) {
        alert(button + " pressed");
    });

    $('#push-me').on('click', function () {

        connection.invoke("ButtonPressed", "arg1");

    });

    connection.start().then(function () {
        console.log("connection started");
    });

});
