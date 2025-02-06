ITComp = function () { }

ITComp.Utils = function () { }

ITComp.Utils.getFunctionByName = function (name, context) {
    if (context === undefined) context = window;

    var nameSpaces = name.split("."),
    tempContext = context,
    nameSpace;

    for (var i = 0; i < nameSpaces.length; i++) {
        nameSpace = nameSpaces[i];

        if (nameSpace in tempContext) {
            tempContext = context[nameSpace];
        }
        else {
            // Object not found;
            return null;
        }
    }
    return tempContext;
};