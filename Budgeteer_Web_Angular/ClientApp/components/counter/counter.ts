import * as ng from "@angular/core";

@ng.Component({
    selector: "counter",
    template: require("./counter.html")
})
export class Counter {
    currentCount = 0;

    incrementCounter() {
        this.currentCount++;
    }
}