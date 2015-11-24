//обработчик события выбора автобуса
function SelectNumber()
{
    var busNumber = $("#Bus :selected");
    $("#stopsContainer").hide();
    if (busNumber.val() != "") {
        startLoadingAnimation();
        $.getJSON("/Home/GetStopsNames" + "?busNumber=" + encodeURIComponent(busNumber.text()), null, GetData);
    }
    else {
        $("#Stop").text("");
        $("#EndStop").text("");
        $("#Days").text("");
    }
}
function GetData(result) {
    $("#Stop").text("");
    $("#Stop").append("<option>" + "</option>")
    $("#EndStop").text("");
    $("#EndStop").append("<option>" + "</option>")
    $("#Days").text("");
    $("#Days").append("<option>" + "</option>")
    $.each(result, function (i) { $("#Stop").append("<option>" + this + "</option>") })
    stopLoadingAnimation();
}

//обработчик события выбора остановки
function SelectStop()
{
    var stop = $("#Stop :selected");
    var busNumber = $("#Bus :selected");
    $("#stopsContainer").hide();
    if (stop.val() != "" && busNumber.val() != "") {
        startLoadingAnimation();
        $.getJSON("/Home/GetFinalStops" + "?stopName=" + encodeURIComponent(stop.text()) + "&busNumber=" + encodeURIComponent(busNumber.text()), null, GetFinalStops);
    }
    else {
        $("#EndStop").text("");
        $("#Days").text("");
    }
}
function GetFinalStops(result) {
    $("#EndStop").text("");
    $("#EndStop").append("<option>" + "</option>")
    $("#Days").text("");
    $("#Days").append("<option>" + "</option>")
    $.each(result, function (i) { $("#EndStop").append("<option>" + this + "</option>") })
    stopLoadingAnimation();
}

//обработчик события выбора конечной остановки
function SelectEndStop()
{
    var endStop = $("#EndStop :selected");
    var stop = $("#Stop :selected");
    var busNumber = $("#Bus :selected");
    $("#stopsContainer").hide();
    if (stop.val() != "" && busNumber.val() != "" && endStop.val() != "")
    {
        startLoadingAnimation();
        $.getJSON("/Home/GetDays" + "?stopName=" + encodeURIComponent(stop.text()) + "&busNumber=" + encodeURIComponent(busNumber.text()) + "&endStop=" + encodeURIComponent(endStop.text()), null, GetDays);
    }
    else
    {
        $("#Days").text("");
    }
}
function GetDays(days)
{
    $("#Days").text("");
    $("#Days").append("<option>" + "</option>")
    $.each(days.result, function (i) { $("#Days").append("<option>" + this + "</option>") })
    stopLoadingAnimation();
}



//---------------------------------------------------------------------------------------------
function SelectAll() {
    var busNumber = $("#Bus");
    var stopName = $("#Stop");
    var endStopName = $("#EndStop");
    var days = $("#Days");

    if (busNumber.val() != "" & stopName.val() != "" & endStopName.val() != "" & days.val() != "")
    {
        startLoadingAnimation();
        $.getJSON("/Home/GetStops" + "?busNumber=" + encodeURIComponent(busNumber.val()) + "&stopName=" + encodeURIComponent(stopName.val()) + "&endStopName=" + encodeURIComponent(endStopName.val()) + "&days=" + encodeURIComponent(days.val()), null, GetNodes);
        $("#stopsContainer").show();
    }
    else
    {
        $("#stopsContainer").hide();
        $("#nodes").text("");
    }
}

function GetNodes(nodes) {
    $("#nodes").text("");
    if (nodes.stops.length != 0) {
        $.each(nodes.stops, function (i) { $("#nodes").append("<span class='breaks'>" + this + "</span> ") })
    }
    else {
        $("#nodes").append("<span class='breaks'> Нет рейсов</span>")
    }
    stopLoadingAnimation();
}
//------------------------------------------------------------------------------------
function startLoadingAnimation() // - функция запуска анимации
{
    var imgObj = $("#loadImg");
    var jumbotron = $(".jumbotron");
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

function stopLoadingAnimation() // - функция останавливающая анимацию
{
    $("#loadImg").hide(400);
    $("#loadBackground").hide(400);
}