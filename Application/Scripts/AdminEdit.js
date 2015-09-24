//обработчик события выбора автобуса
function selectNumber() {
    var busNumber = document.getElementById("busNumber");
    if (busNumber.value != "") {
        startLoadingAnimation();
        $("#stops").text("");
        $.getJSON("/Home/GetStopsNames" + "?busNumber=" + encodeURIComponent(busNumber.value), null, getData);
    }
    else {
        $("#stops").text("");
        $("#stopName").text("");
        $("#finalStop").text("");
        $("#days").text("");
        if (document.getElementById("stopsContainer").hasAttribute("hidden") != true) {
            document.getElementById("stopsContainer").setAttribute("hidden", "")
        }
    }
}
function getData(result) {
    $("#stops").text("");
    $("#stopName").text("");
    $("#stopName").append("<option>" + "</option>")
    $("#finalStop").text("");
    $("#finalStop").append("<option>" + "</option>")
    $("#days").text("");
    $("#days").append("<option>" + "</option>")
    if (document.getElementById("stopsContainer").hasAttribute("hidden") != true) {
        document.getElementById("stopsContainer").setAttribute("hidden");
    }
    $.each(result, function (i) { $("#stopName").append("<option>" + this + "</option>") })
    stopLoadingAnimation();
}

//---------------------------------------------------------------------------------------------
function selectAll() {
    var busNumber = document.getElementById("busNumber");
    var stopName = document.getElementById("stopName");
    var endStopName = document.getElementById("finalStop");
    var days = document.getElementById("days");

    if (busNumber.value != "" & stopName.value != "" & endStopName.value != "" & days.value != "") {
        startLoadingAnimation();
        $.getJSON("/Home/GetStops" + "?busNumber=" + encodeURIComponent(busNumber.value) + "&stopName=" + encodeURIComponent(stopName.value) + "&endStopName=" + encodeURIComponent(endStopName.value) + "&days=" + encodeURIComponent(days.value), null, GetNodes);
        if (document.getElementById("stopsContainer").hasAttribute("hidden") == true) {
            document.getElementById("stopsContainer").removeAttribute("hidden");
        }
    }
    else {
        if (document.getElementById("stopsContainer").hasAttribute("hidden") != true) {
            document.getElementById("stopsContainer").setAttribute("hidden");
        }
        $("stops").text("");
    }

}

function GetNodes(nodes) {
    $("#stops").text("");
    if (nodes.length != 0) {
        $.each(nodes, function (i) { $("#stops").append(this + " ") })
    }
    else {
        $("#stops").append("Нет рейсов")
    }
    stopLoadingAnimation();
}

//-----------------------------------------------------------------------------------------------

//обработчик события выбора остановки
function selectStop() {
    $("#finalStop").text("");
    $("#finalStop").append("<option>" + "</option>")
    $("#days").text("");
    $("#days").append("<option>" + "</option>")
    var stopName = document.getElementById("stopName");
    var busNumber = document.getElementById("busNumber");
    if (stopName.value != "") {
        startLoadingAnimation();
        $.getJSON("/Home/GetFinalStops" + "?stopName=" + encodeURIComponent(stopName.value) + "&busNumber=" + encodeURIComponent(busNumber.value), null, GetendStops);
    }
}

function GetendStops(stops) {

    $("#finalStop").text("");
    $("#finalStop").append("<option>" + "</option>")
    $.each(stops, function (i) { $("#finalStop").append("<option>" + this + "</option>") })

    var finalStop = document.getElementById("finalStop");
    finalStop.options.selectedIndex = 1;
    selectendStop();

    stopLoadingAnimation();
}


function selectendStop() {
    if (document.getElementById("finalStop").value != "") {
        startLoadingAnimation();
        var stopName = document.getElementById("stopName");
        var busNumber = document.getElementById("busNumber");
        var endStopName = document.getElementById("finalStop");
        $.getJSON("/Home/GetDays" + "?stopName=" + encodeURIComponent(stopName.value) + "&busNumber=" + encodeURIComponent(busNumber.value) + "&endStop=" + encodeURIComponent(endStopName.value), null, GetDays);
    }
}



function GetDays(days) {
    $("#days").text("");
    $("#days").append("<option>" + "</option>")
    $.each(days.result, function (i) { $("#days").append("<option>" + this + "</option>") })

    var alldays = document.getElementById("days");

    for (i = 0; i < alldays.length; i++) {
        if (alldays.options[i].value == days.now) {
            alldays.options.selectedIndex = i;
        }
    }
    selectAll();
    stopLoadingAnimation();
}
//---------------------------------------------------------------------------


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