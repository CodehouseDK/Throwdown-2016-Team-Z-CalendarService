var socket = require('./sockets.js');
var personcalendar = require('./personcalendar.js');
var vacationcalendar = require('./vacationcalendar.js');

var socketEvents = socket('localhost:5005').then(events => {
    events.on('message', data => {
        if (!data) {
            return;
        }
        var obj = JSON.parse(data);
        switch(obj.Type) {
            case 'PersonStateModel':
                personcalendar.render(obj);
                break;

            case 'VacationStateModel':
                vacationcalendar.render(obj);
                break;
        }
    });
});

vacationcalendar.update();