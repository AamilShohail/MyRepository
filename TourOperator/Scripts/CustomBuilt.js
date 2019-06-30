/// <reference path="jquery-3.3.1.js" />

$(function () {
    $("#btnsubmit").mouseover(function () {
        $("#btnsubmit").css("backgroundColor", "green");
    });

    $("#btnsubmit").mouseout(function () {
        $("#btnsubmit").css("backgroundColor", "grey");
    });
});