function SelectRoute()
{
    var item = $("#userRoutes");
    $.get("/Routes/SelectRoutes" + "?Name=" + encodeURIComponent(item.val()), null, getRoutes);
}


function getRoutes(value)
{
    var item = $("#Routes");
    item.html(value);
       
}