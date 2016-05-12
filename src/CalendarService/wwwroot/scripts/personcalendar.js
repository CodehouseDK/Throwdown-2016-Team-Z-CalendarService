require("../style/widget.css");
module.exports = function () {
    var id = 'calendar-widget';
    var instance = {};
    var timeout;

    instance.getElement = function () {
        var elm = document.getElementById(id);
        return elm;
    }

    instance.render = function (data) {
        if (timeout) {
            clearTimeout(timeout);
            timeout = null;
        }

        var html = '';
        if (data.CurrentUsername) {
            html += '<header class="widget-header"><h2>Hi ' + data.CurrentUsername + '</h2></header>';
            html += '<div class="widget-body">';

            if (data.Entries && data.Entries.length > 0) {
                html += '<ul>';
                data.Entries.forEach(itm => {
                    html += '<li>';
                    html += '<span class="start date">' + itm.StartText + '</span> ';
                    html += '<strong class="subject">' + itm.Subject + '</strong>';
                    html += '</li>';
                });
                html += '</ul>';
            } else {
                html += '<big>Congratulations, no meetings!</big>';
            }

            html += '</div>';
        }

        var elm = instance.getElement();
        elm.innerHTML = html;

        timeout = setTimeout(function () {
            instance.render({});
        }, 60 * 1000);
    };

    instance.init = function (elementId) {
        id = elementId || id;
    };

    return instance;

}();