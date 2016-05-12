require("../style/widget.css");
module.exports = function () {
    var instance = {};

    document.write('<section id="vacation-widget" class="widget"></section>');

    instance.getElement = function() {
        var elm = document.getElementById('vacation-widget');
        return elm;
    }

    instance.update = function() {
        var xhr = new XMLHttpRequest();
        xhr.open('GET', '/calendar/vacation');
        xhr.onreadystatechange = function() {
            if (xhr.readyState !== 4) {
                return;
            }

            if (xhr.status !== 200) {
                return;
            }

            var json = xhr.responseText;
            var obj = JSON.parse(json);
            instance.render(obj);
        };
        xhr.send(null);
    };

    instance.render = function(data) {
        var html = '';
        html += '<header class="widget-header"><h2>Who is rebooting</h2></header>';
        html += '<div class="widget-body">';

        if (data.Entries) {
            html += '<ul>';
            data.Entries.forEach(itm => {
                html += '<li><strong class="person">' + itm.Username + '</strong> <span class="from">' + itm.StartText + '</span> - <span class="to">' + itm.EndText + '</span> <span class="subject">' + itm.Subject + '</span>,<span class="location">' + itm.Location + '</span></li>';
            });
            html += '</ul>';
        } else {
            html += '<big>No one!!!! :o</big>';
        }

        html += '</div>';

        var elm = instance.getElement();
        elm.innerHTML = html;
    };

    return instance;

}();