var socket = require('./sockets.js');
var widget = require('./widget.js');

var socketEvents = socket('localhost:5005').then(events => {
    events.on('message', data => {

        var obj = data ? JSON.parse(data) : null;
        widget.render(obj);
    });
});