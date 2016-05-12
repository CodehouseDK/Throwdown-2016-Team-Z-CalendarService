var events = require('events');
var eventEmitter = new events.EventEmitter();
var Promise = require("promise");

module.exports = (url) => {

    return new Promise((resolve, reject) => {
        var socket;

        try {
            socket = new WebSocket("ws://" + url);
        } catch (error) {
            reject(error);
            return;
        }

        socket.onmessage = (event) => {
            console.log('Received:');
            console.log(event);
            eventEmitter.emit('message', event.data);
        }
        eventEmitter.on("send", data => {
            socket.send(data);
        });

        socket.onerror = (event) => {
            console.log('error:');
            console.log(event);
            eventEmitter.emit('error', event.data);
        }

        socket.onclose = (event) => {
            console.log('error:');
            console.log(event);
            eventEmitter.emit('error', event.data);
        }

        socket.onopen = () => {
            console.log('connected');
            eventEmitter.emit('connection-open', "Connection to " + url);
            return resolve(eventEmitter);
        };
    });

};