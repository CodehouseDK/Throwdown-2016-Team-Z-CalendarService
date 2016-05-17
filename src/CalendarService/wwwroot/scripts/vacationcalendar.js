require("../style/widget.css");
module.exports = function () {
    var id = 'vacation-widget';
    var instance = {};

    var rooturl = '/';

    instance.getElement = function () {
        var elm = document.getElementById(id);
        return elm;
    }

    instance.update = function () {
        var xhr = new XMLHttpRequest();
        xhr.open('GET', rooturl + 'calendar/vacation');
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
                html += '<img src="https://throwdown2016.blob.core.windows.net/codehousethrowdown/' + itm.Username + '.png" />';
                html += '<span class="description">';
                html += '<span class="periode"><span class="date">' + itm.StartText + '</span> - <span class="date">' + itm.EndText + '</span></span>';
                html += '<span class="subject">' + itm.Subject + '</span>';
                if (itm.Location) {
                    html += '<span class="location">' + itm.Location + '</span>';
                }

                html += '</span></li>';
            });
            html += '</ul>';
        } else {
            html += '<big>No one!!!! :o</big>';
        }

        html += '</div>';

        var elm = instance.getElement();
        elm.innerHTML = html;
    };

    instance.init = function (elementId, url) {
        id = elementId || id;
        if (url && url.indexOf('http') < 0) {
            url = 'http://' + url;
        }
        if (url && url.substring(url.length - 1) !== '/') {
            url += '/';
        }

        rooturl = url;
    }

    return instance;

}();