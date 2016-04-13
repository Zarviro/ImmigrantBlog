$(document).ready(function () {

    $('#searchForm').submit(function () {
        if ($("#search").val().trim())
            return true;
        return false;
    });

});