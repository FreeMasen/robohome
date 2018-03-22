import { Listener } from './listener';
import * as Rabbit from 'amqplib';

let listener = new Listener();

listener.on('new_message', (msg: any) => {
    let payload = JSON.parse(msg.content.toString());
    console.log('new_message', payload);
});