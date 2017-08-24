import { readFileSync, readdir } from 'fs';
import * as path from 'path';
export class Settings {
    mqConnectionString: string;
    remoteId: string;
    constructor() {
        let settingsText: string = readFileSync('./appsettings.json', 'utf8');
        let settings: any = JSON.parse(settingsText);
        this.mqConnectionString = settings.mqConnectionString;
        this.remoteId = process.env.ROBOHOME_REMOTE_ID;
    }
}