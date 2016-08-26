import { Injectable } from "@angular/core";
import { Http, Response } from "@angular/http";

import { Observable } from "rxjs/Observable";
import "rxjs/add/operator/map";
import "rxjs/add/operator/catch";

import { ChartData, ColumnData, PieData } from "./chart-view-model";
import { HelperService } from "./helper.service";

@Injectable()
export class ChartService {
    private chartsUrl = "api/chart"; // URL to web api
    private chartsQuery = ""; // query for web api

    constructor(
        private http: Http,
        private helperService: HelperService) {
    }

    getChartData(chartName: string,
        dateFrom: Date,
        dateUntil: Date,
        personName: string = null,
        categoryName: string = null): Observable<ChartData> {

        this.chartsQuery = "?chartName=" + chartName + this.dateString(dateFrom, dateUntil) + "&personName=" + (personName || "") + "&categoryName=" + (categoryName || "");
        console.log(this.chartsQuery);

        switch (chartName) {
            case "OverviewLeftChart":
            case "SpendingTopLeftChart":
            case "SpendingTopRightChart":
            case "IncomeTopLeftChart":
            case "IncomeTopRightChart":
                return this.getColumnChartData();

            case "OverviewRightChart":
            case "SpendingBottomLeftChart":
            case "SpendingBottomRightChart":
            case "IncomeBottomLeftChart":
            case "IncomeBottomRightChart":
                return this.getPieChartData();

            default:
                return null;
        }
    }

    getColumnChartData(): Observable<ColumnData> {
        return (this.http.get(this.chartsUrl + this.chartsQuery)
            .map(this.helperService.extractData)
            .catch(this.helperService.handleError));
    }

    getPieChartData(): Observable<PieData> {
        return (this.http.get(this.chartsUrl + this.chartsQuery)
            .map(this.helperService.extractData)
            .catch(this.helperService.handleError));
    }

    dateString(dateFrom, dateUntil): string {
        return "&dateFrom=" +
            (dateFrom.getMonth() + 1) +
            "%2F" +
            dateFrom.getDate() +
            "%2F" +
            dateFrom.getFullYear() +
            "%2000%3A00%3A00" +
            "&dateUntil=" +
            (dateUntil.getMonth() + 1) +
            "%2F" +
            dateUntil.getDate() +
            "%2F" +
            dateUntil.getFullYear() +
            "%2000%3A00%3A00";
    }
}