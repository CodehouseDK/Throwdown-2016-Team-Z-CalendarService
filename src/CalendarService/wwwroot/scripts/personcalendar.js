require("../style/widget.css");
module.exports = function () {
    var instance = {};

    document.write('<section id="calendar-widget" class="widget calendar-widget"></section>');

    instance.getElement = function () {
        var elm = document.getElementById('calendar-widget');
        return elm;
    }

    instance.render = function (data) {
        var html = '';
        if (data.CurrentUsername) {
            html += '<header class="widget-header"><h2>Hi ' + data.CurrentUsername + '</h2></header>';
            html += '<div class="widget-body">';

            if (data.Entries && data.Entries.length > 0) {
                html += '<ul>';
                data.Entries.forEach(itm => {
                    html += '<li><span class="start date">' + itm.StartText + '</span> <strong class="subject">' + itm.Subject + '</strong></li>';
                });
                html += '</ul>';
            } else {
                html += '<big>Congratulations, no meetings!</big>';
            }

            html += '</div>';
        }

        var elm = instance.getElement();
        elm.innerHTML = html;
    };

    return instance;

}();