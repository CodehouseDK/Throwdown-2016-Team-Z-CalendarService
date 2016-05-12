require("../style/widget.css");
module.exports = function () {
    var id = 'vacation-widget';
    var instance = {};

    instance.getElement = function () {
        var elm = document.getElementById(id);
        return elm;
    }

    instance.update = function () {
        var xhr = new XMLHttpRequest();
        xhr.open('GET', '/calendar/vacation');
        xhr.onreadystatechange = function () {
            if (xhr.readyState !== 4) {
                return;
            }

            if (xhr.status !== 200) {
                setTimeout(instance.update, 500);
                return;
            }

            var json = xhr.responseText;
            var obj = JSON.parse(json);
            instance.render(obj);
        };
        xhr.send(null);
    };

    instance.render = function (data) {
        var html = '';
        html += '<header class="widget-header"><h2>Who is rebooting</h2></header>';
        html += '<div class="widget-body">';

        if (data.Entries) {
            html += '<ul>';
            data.Entries.forEach(itm => {
                html += '<li>';
                html += '<strong class="person">' + itm.Username + '</strong> ';
                html += '<span class="from">' + itm.StartText + '</span>';
                html += ' - ';
                html += '<span class="to">' + itm.EndText + '</span> ';
                html += '<span class="subject">' + itm.Subject + '</span>';
                if (itm.Location) {
                    html += ', <span class="location">' + itm.Location + '</span>';
                }

                html += '</li>';
            });
            html += '</ul>';
        } else {
            html += '<big>No one!!!! :o</big>';
        }

        html += '</div>';

        var elm = instance.getElement();
        elm.innerHTML = html;
    };

    instance.init = function (elementId) {
        id = elementId || id;
        instance.update();
    }

    return instance;

}();