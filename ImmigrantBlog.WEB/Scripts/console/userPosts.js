$(document).ready(function () {

    $(".pagination").on("click", function (e) {
        e.preventDefault();
        $.ajax({
            url: $(this).attr("href"),
            type: "GET",
            contentType: "application/json;charset=utf-8",
            success: function (result) {
                $("#userPosts").html(result);
            },
            error: function (x, y, z) {
                alert(x + '\n' + y + '\n' + z);
            }
        });
    });

});