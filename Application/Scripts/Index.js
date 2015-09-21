//обработчик события выбора автобуса
function selectNumber() {
    var busNumber = document.getElementById("busNumber");
    if (busNumber.value != "") {
        startLoadingAnimation();
        $("#nodes").text("");
        $.getJSON("/Home/GetStopsNames" + "?busNumber=" + encodeURIComponent(busNumber.value), null, getData);
    }
    else {
        $("#nodes").text("");
        $("#stopName").text("");
        $("#endStop").text("");
        $("#days").text("");
        if (document.getElementById("otherBusContainer").hasAttribute("hidden") != true) {
            document.getElementById("otherBusContainer").setAttribute("hidden", "")
        }
        if (document.getElementById("stopsContainer").hasAttribute("hidden") != true) {
            document.getElementById("stopsContainer").setAttribute("hidden", "")
        }
        if (document.getElementById("nearestTimeContainer").hasAttribute("hidden") != true) {
            document.getElementById("nearestTimeContainer").setAttribute("hidden", "")
        }
    }
}
function getData(result) {
    $("#nodes").text("");
    $("#stopName").text("");
    $("#stopName").append("<option>" + "</option>")
    $("#endStop").text("");
    $("#endStop").append("<option>" + "</option>")
    $("#days").text("");
    $("#days").append("<option>" + "</option>")
    if (document.getElementById("stopsContainer").hasAttribute("hidden") != true) {
        document.getElementById("stopsContainer").setAttribute("hidden", "");
    }
    if (document.getElementById("otherBusContainer").hasAttribute("hidden") != true) {
        document.getElementById("otherBusContainer").setAttribute("hidden", "");
    }
    if (document.getElementById("nearestTimeContainer").hasAttribute("hidden") != true) {
        document.getElementById("nearestTimeContainer").setAttribute("hidden", "");
    }
    $.each(result, function (i) { $("#stopName").append("<option>" + this + "</option>") })
    stopLoadingAnimation();
    //$.each(result.Days, function (i) { $("#days").append("<option>" + this + "</option>") })
}

//---------------------------------------------------------------------------------------------
function selectAll() {
    var busNumber = document.getElementById("busNumber");
    var stopName = document.getElementById("stopName");
    var endStopName = document.getElementById("endStop");
    var days = document.getElementById("days");

    if (busNumber.value != "" & stopName.value != "" & endStopName.value != "" & days.value != "") {
        startLoadingAnimation();
        $.getJSON("/Home/GetStops" + "?busNumber=" + encodeURIComponent(busNumber.value) + "&stopName=" + encodeURIComponent(stopName.value) + "&endStopName=" + encodeURIComponent(endStopName.value) + "&days=" + encodeURIComponent(days.value), null, GetNodes);
        if (document.getElementById("stopsContainer").hasAttribute("hidden") == true) {
            document.getElementById("stopsContainer").removeAttribute("hidden");
        }
        if (document.getElementById("nearestTimeContainer").hasAttribute("hidden") == true) {
            document.getElementById("nearestTimeContainer").removeAttribute("hidden");
        }
    }
    else {
        if (document.getElementById("stopsContainer").hasAttribute("hidden") != true) {
            document.getElementById("stopsContainer").setAttribute("hidden", "");
        }
        if (document.getElementById("nearestTimeContainer").hasAttribute("hidden") != true) {
            document.getElementById("nearestTimeContainer").setAttribute("hidden", "");
        }
        $("#nodes").text("");
        $("#nearestTime").text("");
    }

}

function GetNodes(nodes) {
    $("#nodes").text("");
    $("#nearestTime").text("");
    if (nodes.stops.length != 0) {
        $.each(nodes.stops, function (i) { $("#nodes").append("<span class='breaks'>" + this + "</span> ") })
        $("#nearestTime").append(nodes.nearestStop)
    }
    else {
        $("#nearestTime").append("<span class='breaks'> Нет рейсов</span>")
        $("#nodes").append("<span class='breaks'> Нет рейсов</span>")
    }
    stopLoadingAnimation();
}

//-----------------------------------------------------------------------------------------------

//обработчик события выбора остановки
function selectStop() {
    $("#endStop").text("");
    $("#endStop").append("<option>" + "</option>")
    $("#days").text("");
    $("#days").append("<option>" + "</option>")
    var stopName = document.getElementById("stopName");
    var busNumber = document.getElementById("busNumber");
    if (stopName.value != "") {
        startLoadingAnimation();
        $.getJSON("/Home/GetOtherBuses" + "?stopName=" + encodeURIComponent(stopName.value) + "&busNumber=" + encodeURIComponent(busNumber.value), null, GetOtherBuses);
        $.getJSON("/Home/GetFinalStops" + "?stopName=" + encodeURIComponent(stopName.value) + "&busNumber=" + encodeURIComponent(busNumber.value), null, GetFinalStops);
    }
    else {
        if (document.getElementById("otherBusContainer").hasAttribute("hidden") == false) {
            document.getElementById("otherBusContainer").setAttribute("hidden", "")
        }
    }
}

function GetFinalStops(stops) {

    $("#endStop").text("");
    $("#endStop").append("<option>" + "</option>")
    $.each(stops, function (i) { $("#endStop").append("<option>" + this + "</option>") })

    var endStop = document.getElementById("endStop");
    endStop.options.selectedIndex = 1;
    selectFinalStop();
    stopLoadingAnimation();
}



function GetOtherBuses(buses) {
    if (buses != 0) {
        if (document.getElementById("otherBusContainer").hasAttribute("hidden") == true) {
            document.getElementById("otherBusContainer").removeAttribute("hidden");
        }

        $("#otherBuses").text("");
        $.each(buses, function (i) {
            var elem = document.createElement('span');
            elem.setAttribute('class', 'breaks');
            elem.innerHTML = this;
            elem.setAttribute("onclick", "selectOtherBusOnThisStop(" + "'" + this + "'" + ")");
            $("#otherBuses").append(elem);
            $("#otherBuses").append(" ");
        })
    }
    else {
        if (document.getElementById("otherBusContainer").hasAttribute("hidden") == false) {
            document.getElementById("otherBusContainer").setAttribute("hidden");
        }
    }
}


function selectFinalStop() {
    if (document.getElementById("endStop").value != "") {
        startLoadingAnimation();
        var stopName = document.getElementById("stopName");
        var busNumber = document.getElementById("busNumber");
        var finalStopName = document.getElementById("endStop");
        $.getJSON("/Home/GetDays" + "?stopName=" + encodeURIComponent(stopName.value) + "&busNumber=" + encodeURIComponent(busNumber.value) + "&endStop=" + encodeURIComponent(finalStopName.value), null, GetDays);
    }
    else {
        $("#days").text("");
        $("#days").append("<option>" + "</option>")
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


//обработчик выбора другого автобуса на этой остановке
function selectOtherBusOnThisStop(_busNumber) {
    startLoadingAnimation();
    var val = this.value;
    $.getJSON("/Home/GetStopsNames" + "?busNumber=" + encodeURIComponent(_busNumber), null, GetNewData);
    var busNumber = document.getElementById("busNumber");
    for (i = 0; i < busNumber.length; i++) {
        if (busNumber.options[i].value == _busNumber) {
            busNumber.options.selectedIndex = i;
        }
    }
}


function GetNewData(result) {
    $("#nodes").text("");
    var stopName = document.getElementById("stopName").value;
    $("#stopName").text("");
    $("#stopName").append("<option>" + "</option>")
    $("#endStop").text("");
    $("#endStop").append("<option>" + "</option>")
    $("#days").text("");
    $("#days").append("<option>" + "</option>")
    if (document.getElementById("stopsContainer").hasAttribute("hidden") != true) {
        document.getElementById("stopsContainer").setAttribute("hidden", "")
    }
    if (document.getElementById("otherBusContainer").hasAttribute("hidden") != true) {
        document.getElementById("otherBusContainer").setAttribute("hidden", "")
    }
    if (document.getElementById("nearestTimeContainer").hasAttribute("hidden") != true) {
        document.getElementById("nearestTimeContainer").setAttribute("hidden", "")
    }
    $.each(result, function (i) { $("#stopName").append("<option>" + this + "</option>") })
    var ttt = document.getElementById("stopName");
    for (i = 0; i < ttt.length; i++) {
        var eee = ttt.options[i].value;
        if (eee == stopName) {
            ttt.options.selectedIndex = i;
        }
    }
    stopLoadingAnimation();
    selectStop();

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