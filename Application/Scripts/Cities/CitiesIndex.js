function SelectCity()
{
    var city = $("#city").val();
    var returnUrl = window.location.href;
    $.get("/Cities/SetCity", { city: city }, function () { location.reload(true);})
}
