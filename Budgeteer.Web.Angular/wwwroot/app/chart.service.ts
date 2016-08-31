import { Injectable } from "@angular/core";
import { Http, Response } from "@angular/http";

import { Observable } from "rxjs/Observable";
import "rxjs/add/operator/map";
import "rxjs/add/operator/catch";

import { ColumnData, PieData } from "./chart-view-model";
import { HelperService } from "./helper.service";

@Injectable()
export class ChartService {
    private chartsUrl = "api/charts"; // URL to web api
    private chartsQuery = ""; // query for web api

    constructor(
        private http: Http,
        private helperService: HelperService) {
    }

    getColumnChartData(chartName: string,
        dateFrom: Date,
        dateUntil: Date,
        personName: string = null,
        categoryName: string = null): Observable<ColumnData> {

        this.chartsQuery = `?chartName=${chartName}${this
            .dateString(dateFrom, dateUntil)}&personName=${personName || ""}&categoryName=${categoryName || ""}`;

        return (this.http.get(this.chartsUrl + this.chartsQuery)
            .map(this.helperService.extractData)
            .catch(this.helperService.handleError));
    }

    getPieChartData(chartName: string,
        dateFrom: Date,
        dateUntil: Date,
        personName: string = null,
        categoryName: string = null): Observable<PieData> {

        this.chartsQuery = `?chartName=${chartName}${this
            .dateString(dateFrom, dateUntil)}&personName=${personName || ""}&categoryName=${categoryName || ""}`;

        return (this.http.get(this.chartsUrl + this.chartsQuery)
            .map(this.helperService.extractData)
            .catch(this.helperService.handleError));
    }

    private dateString(dateFrom, dateUntil): string {
        dateFrom = new Date(dateFrom);
        dateUntil = new Date(dateUntil);

        return `&dateFrom=${dateFrom.getMonth() + 1}%2F${dateFrom.getDate()}%2F${dateFrom
            .getFullYear()}%2000%3A00%3A00&dateUntil=${dateUntil.getMonth() + 1}%2F${dateUntil.getDate()}%2F${dateUntil
            .getFullYear()}%2000%3A00%3A00`;
    }
}