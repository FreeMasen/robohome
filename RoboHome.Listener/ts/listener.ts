import { Settings } from './settings';
import * as Rabbit from 'amqplib/callback_api';
import {Replies} from 'amqplib';
import * as events from 'events';

export class Listener  extends events {
    settings: Settings;
    connection: Rabbit.Connection;
    channel: Rabbit.Channel;
    exchange: string;

    constructor(){
        super();
        this.settings = new Settings();
        this.exchange = 'switches';
        this.start();
    }

    async start() {
        this.connection = await this.connect();
        this.channel = await this.createChannel();
        this.channel.assertExchange(this.exchange, 'topic',{durable: false});
        let queue = await this.assertQueue();

        this.channel.bindQueue(queue.queue, this.exchange, this.settings.remoteId);
        let msg = this.consume(queue)
    }

    async connect(): Promise<Rabbit.Connection> {
        return await new Promise<Rabbit.Connection>((resolve, reject) => {
            Rabbit.connect(this.settings.mqConnectionString, (err, connection) => {
                if (err) return reject(err);
                return resolve(connection);
            })
        });
    }

    async createChannel(): Promise<Rabbit.Channel> {
        return await new Promise<Rabbit.Channel>((resolve, reject) => {
            this.connection.createChannel((err, channel) => {
                if (err) return reject(err);
                return resolve(channel);
            });
        });
    }

    async assertQueue(): Promise<Replies.AssertQueue> {
        return await new Promise<Replies.AssertQueue>((resolve, reject) => {
            this.channel.assertQueue(this.exchange, {durable: false}, (err, ok) => {
                if (err) return reject(err);
                return resolve(ok);
            });
        });
    }

    async consume(queue: Replies.AssertQueue): Promise<any> {
        return await new Promise<any>((resolve, reject) => {
            this.channel.consume(queue.queue, msg => {
                return resolve(msg);
            })
        });
    }
}