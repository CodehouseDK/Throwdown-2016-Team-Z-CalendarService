var socket = require('./sockets.js');
var personcalendar = require('./personcalendar.js');
var vacationcalendar = require('./vacationcalendar.js');

var socketEvents;

window.calendarservice = {
    init: function (rooturl, vacationid, personcalendarid) {
        vacationcalendar.init(vacationid, rooturl);
        personcalendar.init(personcalendarid);

        socketEvents = socket(rooturl).then(events => {
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
                .on('connection-open', () => {
                    //vacationcalendar.update();
                });
        });

    }
};
