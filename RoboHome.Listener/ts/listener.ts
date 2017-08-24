import { Settings } from './settings';
import * as Rabbit from 'amqplib/callback_api';
import * as events from 'events';

export class Listener  extends events {
    settings: Settings;
    connection: Rabbit.Connection;

    constructor(){
        super();
        this.settings = new Settings();
        this.start();
    }

    start(): void {
        Rabbit.connect(this.settings.mqConnectionString, (err, connection) => {
            if (err) throw err;
            connection.createChannel((err, channel) => {
                if (err) throw err;
                var exchange = 'switches';
                channel.assertExchange(exchange, 'topic',{durable: false});
                channel.assertQueue('switches', {durable: true}, (err, ok) => {
                    if (err) throw err;
                    channel.bindQueue(ok.queue, exchange, this.settings.remoteId);
                    channel.consume(ok.queue, msg => {
                        this.emit('new_message', msg);
                    });
                });
            });
        })
    }
}