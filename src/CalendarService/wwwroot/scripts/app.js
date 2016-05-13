var socket = require('./sockets.js');
var personcalendar = require('./personcalendar.js');
var vacationcalendar = require('./vacationcalendar.js');

var socketEvents;

window.calendarservice = {
    init: function (socketurl, vacationid, personcalendarid) {
        vacationcalendar.init(vacationid);
        personcalendar.init(personcalendarid);

        socketEvents = socket(socketurl).then(events => {
            events
                .on('message', data => {
                    if (!data) {
                        return;
                    }
                    var obj = JSON.parse(data);
                    switch (obj.Type) {
                        case 'PersonStateModel':
                            personcalendar.render(obj);
                            break;

                        case 'VacationStateModel':
                            vacationcalendar.render(obj);
                            break;
                    }
                })
                .on('connection-open', data => {
                    vacationcalendar.update();
                });
        });

    }
};
