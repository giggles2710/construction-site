$(document).ready(function () {
    // check to toast when delete success
    if (window.sessionStorage.DeletedStatus == "true") {
        toastr.success(window.sessionStorage.DeletedMessage);
        window.sessionStorage.DeletedStatus = null;
    } else if (window.sessionStorage.DeletedStatus == "false") {
        toastr.error(window.sessionStorage.DeletedMessage)
        window.sessionStorage.DeletedStatus = null;
    } else if (window.sessionStorage.EnableStatus == "true") {
        toastr.success(window.sessionStorage.EnableMessage)
        window.sessionStorage.EnableStatus = null;
    }

    //search section
    $("#dataTable_user th").on('click', function () {
        var sortStr = $(this).data('sort');
        var direction = $(this).data('direction');

        if (!Util.IsNullOrWhiteSpace(sortStr) && !Util.IsNullOrWhiteSpace(direction)) {
            // do sort
            userSupportModel.SortUser(sortStr, direction == "asc");
        }
    });

    // btn search
    $('#dataTables_searchUser').on('click', function () {
        userSupportModel.ViewUser();
    });

    // when change value of number of item shown in the grid
    $('#dataTables_showNumberSelectUser').on('change', function () {
        userSupportModel.ViewUser();
    });

    // before delete, show confirmation dialog
    $('.action-link.remove-link.user').on('click', function () {
        var removeUrl = $(this).data('url');
        var currentViewUrl = userSupportModel.GetCurrentViewUserUrl();
        var message = 'hủy';

        if ($(this).prop('title') == 'Active')
        {
            message = 'cho phép hoạt động trở lại';
        }

        bootbox.dialog({
            message: "Bạn có chắc là muốn " + message +' người dùng này không?',
            title: "Thay đổi trạng thái người dùng",
            buttons: {
                okAction: {
                    label: "Có",
                    className: "btn btn-primary btn-sm",
                    callback: function () {
                        // ajax - remove
                        $.ajax({
                            url: removeUrl,
                            method: "POST",
                            success: function (data) {
                                if (data != null) {
                                    if (data.isResult == true) {                                       
                                        if (message == 'hủy')
                                        {
                                            window.sessionStorage.DeletedStatus = true;
                                            window.sessionStorage.DeletedMessage = 'Hủy người dùng thành công';
                                        }
                                        else
                                        {
                                            window.sessionStorage.EnableStatus = true;
                                            window.sessionStorage.EnableMessage = 'Người dùng đã được hoạt động trở lại';
                                        }
                                        
                                    } else {
                                        window.sessionStorage.DeletedStatus = false;
                                        window.sessionStorage.DeletedMessage = data.result;
                                    }

                                    window.location.href = currentViewUrl;
                                }

                            }, error: function (data) {
                                toastr.error(data.result);
                                window.sessionStorage.IsDeletedStatus = null;
                            }
                        });
                    }
                },
                noAction: {
                    label: "Không",
                    className: "btn btn-default btn-sm",
                    callback: function () {

                    }
                }
            }
        });
    });
});

var userSupportModel = {
    SortUser: function (sortField, isAsc) {
        var $activatePage = $('.pageinate_button.active');
        var page = 1; // page
        if ($activatePage.length > 0)
            page = $activatePage[0].text;
        var itemsOnPage = $('#dataTables_showNumberSelectUser').val(); // items on page
        var searchText = $('#dataTables_show_item_search input[type="search"]').val(); // search text

        window.location.href = staticUrl.viewUser + "?page=" + page + "&itemsPerPage="
            + itemsOnPage + "&searchText=" + searchText + "&sortField=" + sortField + "&isAsc=" + isAsc;
    },
    ViewUser: function () {
        window.location.href = userSupportModel.GetCurrentViewUserUrl();
    },
    GetCurrentViewUserUrl: function () {
    var $activatePage = $('.pageinate_button.active');
    var page = 1; // page
    if ($activatePage.length > 0)
        page = $activatePage[0].text;

    var itemsOnPage = $('#dataTables_showNumberSelectUser').val(); // items on page
    var searchText = $('#dataTables_show_item_search input[type="search"]').val(); // search text
    var sortField = $('#dataTables_sort_field_hidden').val(); // sort field
    var directionField = $('#dataTables_sort_direction_hidden').val(); // direction field

    return staticUrl.viewUser + "?page=" + page + "&itemsPerPage="
        + itemsOnPage + "&searchText=" + searchText + "&sortField=" + sortField + "&isAsc=" + directionField;
    }
};