function SelectRoute()
{
    var item = $("#userRoutes");
    startLoadingRouteAnimation();
    $.get("/Routes/SelectRoutes" + "?Name=" + encodeURIComponent(item.val()), null, getRoutes);
}


function getRoutes(value)
{
    var item = $("#Routes");
    item.html(value);
    stopLoadingRouteAnimation();
       
}


//------------------------------------------------------------------------------------
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