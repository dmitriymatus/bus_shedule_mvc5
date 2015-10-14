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
        startLoadingRouteAnimation();
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
    startLoadingRouteAnimation();
}

//выбор остановки
function selectStop() {
    var stop = $("#Stop");
    var busNumber = $("#BusNumber");
    if (stop.val() != "") {
        startLoadingRouteAnimation();
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
    stopLoadingRouteAnimation();
}


function startLoadingRouteAnimation() // - функция запуска анимации
{
    var imgObj = $("#loadImg");
    var jumbotron = $("#RoutesContainer");
    var imgBackground = $("#loadBackground");

    var position = jumbotron.position();
    var jumboHeight = jumbotron.outerHeight(false);
    var jumboWidth = jumbotron.outerWidth(false);
    imgBackground.css("top", position.top + "px");
    imgBackground.css("left", position.left + "px");
    imgBackground.height(jumboHeight);
    imgBackground.width(jumboWidth);


    var centerY = position.top + ((jumboHeight / 2 - imgObj.height() / 2));
    var centerX = position.left + ((jumboWidth / 2 - imgObj.width() / 2));
    imgObj.css("top", centerY + "px");
    imgObj.css("left", centerX + "px");
    imgBackground.show(400);
    imgObj.show(400);
}

function stopLoadingRouteAnimation() // - функция останавливающая анимацию
{
    $("#loadImg").hide(400);
    $("#loadBackground").hide(400);
}
