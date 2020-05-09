dvml = (function () {

    'use strict';

    function add(containerId, actionUrl) {
        var container = $('#' + containerId);

        if (actionUrl.startsWith("POST")) {
            var parts = actionUrl.split("|");
            var action = parts[1];
            var json = parts[2];
            $.ajax({
                url: action,
                data: json,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                type: "POST",
                cache: false,
                success: function (response) {
                    if (response.success) {
                        container.append(response.html);
                    }
                }
            });
        }
        else {
            $.ajax({
                url: actionUrl,
                type: "GET",
                cache: false,
                success: function (html) {
                    container.append(html);
                }
            });
        }
    };

    function remove(itemId) {
        $('#' + itemId).remove();
    };

    return {
        add: add,
        remove: remove
    };
})();