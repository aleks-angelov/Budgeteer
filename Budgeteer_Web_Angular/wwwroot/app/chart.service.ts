import { Injectable } from "@angular/core";
import { Http, Response } from "@angular/http";

import { Observable } from "rxjs/Observable";
import "rxjs/add/operator/map";
import "rxjs/add/operator/catch";

import { HelperService } from "./helper.service";

@Injectable()
export class ChartService {
    private chartsUrl = "api/chart"; // URL to web api

    constructor(
        private http: Http,
        private helperService: HelperService) {
    }

    getChartData(): Observable<string[]> {
        return (this.http.get(this.chartsUrl)
            .map(this.helperService.extractData)
            .catch(this.helperService.handleError));
    }
}