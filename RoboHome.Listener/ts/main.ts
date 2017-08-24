import { Listener } from './listener';
import * as Rabbit from 'amqplib';

let listener = new Listener();

listener.on('new_message', (msg: any) => {
    debugger
    let payload = JSON.parse(msg.content.toString());
});