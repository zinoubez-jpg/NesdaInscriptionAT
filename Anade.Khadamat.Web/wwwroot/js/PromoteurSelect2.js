// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function getDataAttributesAsObject(el, dataAttrRegex) {
    var data = {};
    [].forEach.call(el.attributes, function (attr) {
        if (dataAttrRegex.test(attr.name)) {
            var camelCaseName = attr.name.substr(5).replace(/-(.)/g, function ($0, $1) {
                return $1.toUpperCase();
            });
            data[camelCaseName] = attr.value;
        }
    });
    return data;
}

function promoteurSelect2SelectionTemplate(repo) {
    if (repo.nin !== undefined) {
        var markup =
            "<div>(" + repo.nin + ") " + repo.fullName + "</div>";

        return markup;
    }

    return repo.text || repo.fullName;
}

function promoteurSelect2ResultTemplate(repo) {
    if (repo.loading) return repo.nin;

    var markup =
        "<li>(" + repo.nin + ") " + repo.fullName + "</li>";

    return markup;
}

function escapeMarkup(markup) {
    return markup;
}

function processResult(data, params) {
    // parse the results into the format expected by Select2
    // since we are using custom formatting functions we do not need to
    // alter the remote JSON data, except to indicate that infinite
    // scrolling can be used
    params.page = params.page || 1;

    return {
        results: data.items,
        pagination: {
            more: (params.page * 10) < data.totalCount
        }
    };
}