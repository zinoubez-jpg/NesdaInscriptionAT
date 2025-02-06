datepicker = function () { }

datepicker.init = function (selector, options) {
    if (!options)
        options = {};

    $(selector).each(function (index, element) {
        var dataAttributesOptions = getDataAttributesAsObject(element, /^data-/);

        $.extend(true, dataAttributesOptions, options);

        $(element).datepicker(dataAttributesOptions);

        $(element).next().click(function () {
            $(element).datepicker("show");
        });
    })
}