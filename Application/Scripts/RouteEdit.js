$(function ()
{
    $("#BusNumber :first").remove();
    $("#Stop :first").remove();
    $("#EndStop :first").remove();
})


//выбор автобуса
function selectNumber() {
    var busNumber = $("#BusNumber");
    if (busNumber.val() != "") {
        $.getJSON("/Home/GetStopsNames" + "?busNumber=" + encodeURIComponent(busNumber.val()), null, GetStops);
    }
    else {
        $("#Stop").empty();
        $("#EndStop").empty();
    }
}
function GetStops(stops) {
    $("#Stop").empty();
    $("#Stop").append("<option>" + "</option>")
    $("#EndStop").empty();
    $.each(stops, function (i) { $("#Stop").append("<option>" + this + "</option>") })
}

//выбор остановки
function selectStop() {
    var stop = $("#Stop");
    var busNumber = $("#BusNumber");
    if (stop.val() != "") {
        $.getJSON("/Home/GetFinalStops" + "?stopName=" + encodeURIComponent(stop.val()) + "&busNumber=" + encodeURIComponent(busNumber.val()), null, GetFinalStops);
    }
    else {
        $("#EndStop").empty();
    }
}

function GetFinalStops(endStops) {
    $("#EndStop").empty();
    $("#EndStop").append("<option>" + "</option>")
    $.each(endStops, function (i) { $("#EndStop").append("<option>" + this + "</option>") })
}



