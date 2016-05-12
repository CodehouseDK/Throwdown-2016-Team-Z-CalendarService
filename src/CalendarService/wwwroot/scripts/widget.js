require("../style/widget.css");
module.exports = function () {
    var instance = {};

    document.write('<section id="calendar-widget" class="widget"></section>');

    instance.getElement = function() {
        var elm = document.getElementById('calendar-widget');
        return elm;
    }

    instance.render = function (data) {
        var html = '';
        if (data) {
            html += '<header class="widget-header"><h2>Hi ' + data.CurrentUsername + '</h2></header>';
            html += '<div class="widget-body">';

            if (data.Entries) {
                html += '<ul>';
                data.Entries.forEach(itm => {
                    html += '<li><span class="start date">' + itm.StartText + '</span> <strong class="subject">' + itm.Subject + '</strong></li>';
                });
                html += '</ul>';
            }

            html += '</div>';
        }

        var elm = instance.getElement();
        elm.innerHTML = html;
    };

    instance.update = function(username) {
        var xhr = new XMLHttpRequest();
        xhr.open('GET', '/calendar/coming/' + username);
        xhr.onreadystatechange = function() {
            var DONE = 4;
            var OK = 200;
            if (xhr.readyState !== DONE) {
                return;
            }

            if (xhr.status !== OK) {
                return;
            }

            var data = JSON.parse(xhr.responseText);
            instance.render(data);
        }
        xhr.send(null);
    };

    return instance;

}();