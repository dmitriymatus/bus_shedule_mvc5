//выбор автобуса
function selectNumber()
{
    var busNumber = $("#BusNumber");
    if(busNumber.val() != "")
    {
        $.getJSON("/Home/GetStopsNames" + "?busNumber=" + encodeURIComponent(busNumber.val()), null, GetStops);
    }
    else
    {
        $("#Stop").empty();
        $("#EndStop").empty();
        $("#Days").empty();
    }
}
function GetStops(stops)
{
    $("#Stop").empty();
    $("#Stop").append("<option>" + "</option>")
    $("#EndStop").empty();
    $("#Days").empty();
    $.each(stops, function (i) { $("#Stop").append("<option>" + this + "</option>") })
}

//выбор остановки
function selectStop()
{
    var stop = $("#Stop");
    var busNumber = $("#BusNumber");
    if (stop.val() != "")
    {
        $.getJSON("/Home/GetFinalStops" + "?stopName=" + encodeURIComponent(stop.val()) + "&busNumber=" + encodeURIComponent(busNumber.val()), null, GetFinalStops);
    }
    else
    {
        $("#EndStop").empty();
        $("#Days").empty();
    }
}

function GetFinalStops(endStops)
{
    $("#EndStop").empty();
    $("#EndStop").append("<option>" + "</option>")
    $("#Days").empty();
    $.each(endStops, function (i) { $("#EndStop").append("<option>" + this + "</option>") })
}


//выбор конечной остановки
function selectFinalStop()
{
    var stop = $("#Stop");
    var busNumber = $("#BusNumber");
    var endStop = $("#EndStop");

    if (endStop.val() != "")
    {
        $.getJSON("/Home/GetDays" + "?stopName=" + encodeURIComponent(stop.val()) + "&busNumber=" + encodeURIComponent(busNumber.val()) + "&endStop=" + encodeURIComponent(endStop.val()), null, GetDays);
    }
}


function GetDays(days)
{
    $("#Days").empty();
    $("#Days").append("<option>" + "</option>");

    $.each(days, function (i) { $("#Days").append("<option>" + this + "</option>") })
}