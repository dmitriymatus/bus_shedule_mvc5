function SelectCity()
{
    var city = $("#city").val();
    $.getJSON("/Cities/SetCity" + "?city=" + encodeURIComponent(city), null, null);
}
