function AddStopToRoute(e, element, container, name)
{
    var select = $("#" + element + " :selected" );
    var value = select.val();
    var text = select.text();
    if(text != "")
    {
        select.remove();
        var RouteTable = $("#" + container);
        RouteTable.append("<li class='list-group-item'>"+"<input type='hidden' name="+ name +" value=" + value + ">" + text + "</li>")
    }
    e = e || window.event;
    e.preventDefault ? e.preventDefault() : (e.returnValue = false)
}