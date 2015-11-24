//обработчик события выбора автобуса
function SelectNumber() {
    var busNumber = $("#Bus :selected");
    if (busNumber.val() != "")
    {
        startLoadingAnimation();
        $.getJSON("/Home/GetStopsNames" + "?busNumber=" + encodeURIComponent(busNumber.text()), null, GetData);
    }
    else
    {
        $("#Stop").text("");
        $("#EndStop").text("");
    }
}
function GetData(result)
{
    $("#Stop").text("");
    $("#Stop").append("<option>" + "</option>")
    $("#EndStop").text("");
    $("#EndStop").append("<option>" + "</option>")
    $.each(result, function (i) { $("#Stop").append("<option>" + this + "</option>") })
    stopLoadingAnimation();
}

//обработчик события выбора остановки
function SelectStop() {
    var stop = $("#Stop :selected");
    var busNumber = $("#Bus :selected");
    if (stop.val() != "" && busNumber.val() != "")
    {
        startLoadingAnimation();
        $.getJSON("/Home/GetFinalStops" + "?stopName=" + encodeURIComponent(stop.text()) + "&busNumber=" + encodeURIComponent(busNumber.text()), null, GetFinalStops);
    }
    else
    {
        $("#EndStop").text("");
    }
}
function GetFinalStops(result)
{
    $("#EndStop").text("");
    $("#EndStop").append("<option>" + "</option>")
    $.each(result, function (i) { $("#EndStop").append("<option>" + this + "</option>") })
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