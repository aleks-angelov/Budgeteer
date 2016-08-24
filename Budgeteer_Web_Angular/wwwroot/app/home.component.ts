import { Component, OnInit } from "@angular/core";
import { Title } from "@angular/platform-browser";

@Component({
    selector: "my-home",
    templateUrl: "app/home.component.html"
})
export class HomeComponent implements OnInit {
    constructor(
        private titleService: Title) {
    }

    ngOnInit() {
        this.titleService.setTitle("Home - Budgeteer");
    }
}