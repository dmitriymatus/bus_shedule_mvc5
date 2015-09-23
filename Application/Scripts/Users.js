$(function () {
    $('div#loading').hide();
    $('#loadLink').click(loadItems);
    $('#searchButton').click(searchButtonClick);
    var searchValue = "All";
    var page = 1;
    var _inCallback = false;
    function loadItems() {
        if (page > -1 && !_inCallback) {
            _inCallback = true;
            page++;
            $('div#loading').show();

            $.ajax({
                type: 'GET',
                url: '/AdminManage/Users/'+ searchValue +'/Page' + page,
                success: function (data, textstatus) {
                    if (data != '') {
                        $("#scrolList").append(data);
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


    function searchButtonClick()
    {
        $("#scrolList").empty();
        if ($('#searchInput').val() == "")
        {
            page = 0;
            searchValue = "All";
            loadItems();
            $('#loadLink').show();
        }
        else
        {
            page = 0;
            searchValue = $('#searchInput').val();
            $('#loadLink').show();
            loadItems();
        }
    }


})