import { Time } from "./time";
/**
 * A cached list of key times
 */
export default class KeyTimes {
    constructor(
        public month?: number,
        public day?: number,
        public year?: number,
        public dawn?: Time,
        public sunset?: Time,
        public dusk?: Time,
    ){
        if (!month){
            let dt = new Date();
            this.month = dt.getMonth();
            this.day = dt.getDate();
            this.year = dt.getFullYear();
        }
    }

    static fromJSON(json: any): KeyTimes {
        return new KeyTimes(
            json.month,
            json.day,
            json.year,
            Time.fromJson(json.dawn.time),
            Time.fromJson(json.sunset.time),
            Time.fromJson(json.dusk.time),
        )
    }

    toJSON() {
        return {
            month: this.month,
            day: this.day,
            year: this.year,
            dawn: this.dawn.toJSON(),
            sunset: this.sunset.toJSON(),
            dusk: this.dusk.toJSON(),
        }
    }

    isOutOfDate(): boolean {
        let today = new Date();
        return this.year !== today.getFullYear() &&
                this.month !== today.getMonth() &&
                this.day !== today.getDate()
    }
}