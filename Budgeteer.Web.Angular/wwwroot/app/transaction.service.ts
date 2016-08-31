import { Injectable } from "@angular/core";
import { Headers, Http, Response, RequestOptions } from "@angular/http";

import { Observable } from "rxjs/Observable";
import "rxjs/add/operator/map";
import "rxjs/add/operator/catch";

import { TransactionViewModel } from "./transaction-view-model";
import { HelperService } from "./helper.service";

@Injectable()
export class TransactionService {
    private transactionsUrl = "api/transactions"; // URL to web api

    constructor(
        private http: Http,
        private helperService: HelperService) {
    }

    getTransactions(): Observable<TransactionViewModel[]> {
        return (this.http.get(this.transactionsUrl)
            .map(this.helperService.extractData)
            .catch(this.helperService.handleError));
    }

    postTransaction(tvm: TransactionViewModel): Observable<TransactionViewModel> {
        const body = JSON.stringify(tvm);
        const headers = new Headers({ 'Content-Type': "application/json" });
        const options = new RequestOptions({ headers: headers });

        return (this.http.post(this.transactionsUrl, body, options).catch(this.helperService.handleError));
    }
}