dvml = (function () {

    'use strict';

    // Polyfill for string.startsWith
    if (!String.prototype.startsWith) {
        Object.defineProperty(String.prototype, 'startsWith', {
            value: function (search, rawPos) {
                var pos = rawPos > 0 ? rawPos | 0 : 0;
                return this.substring(pos, pos + search.length) === search;
            }
        });
    }

    function add(containerId, actionUrl) {
        let container = $('#' + containerId);
        let requestOptions = null;

        if (actionUrl.startsWith("POST")) {
            // We will make an ajax request via POST
            let actionSettings = actionUrl.split("|");

            let url = actionSettings[1];
            let data = actionSettings[2];
            let headers = null;
            let traditional = false;

            // Check if the page has an antiforgery token field
            let antiforgeryTokenField = $('input[name="__RequestVerificationToken"]');
            if (antiforgeryTokenField) {
                traditional = true;
                headers = {
                    RequestVerificationToken: antiforgeryTokenField.val(),
                };
            }

            requestOptions = {
                url: url,
                headers: headers,
                data: data,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                traditional: traditional,
                type: "POST",
                cache: false,
                success: function (response) {
                    if (response.success) {
                        container.append(response.html);
                    }
                }
            }
        }
        else {
            // We will make an ajax request via GET
            requestOptions = {
                url: actionUrl,
                type: "GET",
                cache: false,
                success: function (response) {
                    container.append(response);
                }
            }
        }

        // Perform the request
        $.ajax(requestOptions);

        return {
            container: container[0], // return information about the request
            options: requestOptions  // to enable better testing / debugging.
        };
    };

    function remove(itemId) {
        return $('#' + itemId).remove();
    };

    return {
        add: add,
        remove: remove,
    };
})();