$(document).ready(function() {
    $("#searchString").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/api/ProductRestController/Search",
                dataType: "json",
                data: {
                    searchString: request.term
                },
                success: function (data) {
                    response(data);
                }
            });
        },
        minLength: 1
    });
});
