$(function () {
    $('div#loading').hide();
    $('#loadLink').click(loadItems);
    var page = 0;
    var _inCallback = false;
    $('#NewsContainer').hide();
    loadItems();
    function loadItems() {
        if (page > -1 && !_inCallback) {
            _inCallback = true;
            page++;
            $('#loadLink').hide();
            $('div#loading').show();

            $.ajax({
                type: 'GET',
                url: '/News/GetItems',
                data: { Page: page, City: $("#city").val() },
                success: function (data, textstatus) {
                    if (data != '') {
                        $('#NewsContainer').show();
                        $("#NewsList").append(data);
                        $('#loadLink').show();
                    }
                    else {
                        page = -1;
                        $('#loadLink').hide();
                    }
                    _inCallback = false;
                    $("div#loading").hide();
                }
            });
        }
    }
    // обработка события скроллинга
    $(window).scroll(function () {
        if ($(window).scrollTop() == $(document).height() - $(window).height()) {

            loadItems();
        }
    });
})