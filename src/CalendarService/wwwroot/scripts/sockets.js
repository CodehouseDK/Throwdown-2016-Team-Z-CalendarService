var events = require('events');
var eventEmitter = new events.EventEmitter();
var Promise = require("promise");

module.exports = (url) => {

    var websocket;

    function init() {
        websocket = new WebSocket('ws://' + url);
        websocket.onopen = function (evt) { onOpen(evt) };
        websocket.onclose = function (evt) { onClose(evt) };
        websocket.onmessage = function (evt) { onMessage(evt) };
        websocket.onerror = function (evt) { onError(evt) };
    }

    function onOpen() {
        console.log('Connected to ' + url);
        eventEmitter.emit('connection-open', "Connection to " + url);
    }

    function onClose(event) {
        console.log('Disconnected...');
        console.log(event);

        setTimeout(init, 250);
    }

    function onMessage(event) {
        console.log('Received:', event);
        eventEmitter.emit('message', event.data);
    }

    function onError(event) {
        console.log('error:', event);
    }

    return new Promise((resolve, reject) => {

        try {
            init();
        } catch (error) {
            reject(error);
            return;
        }

        resolve(eventEmitter);

    });

};