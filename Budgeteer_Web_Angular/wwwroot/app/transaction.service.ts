import { Injectable } from "@angular/core";
import { Headers, Http, Response, RequestOptions } from "@angular/http";

import { Observable } from "rxjs/Observable";
import "rxjs/add/operator/map";
import "rxjs/add/operator/catch";
import "rxjs/add/observable/throw";

import { TransactionViewModel } from "./transaction-view-model";

@Injectable()
export class TransactionService {
    private transactionsUrl = "api/transactions"; // URL to web api

    constructor(private http: Http) { }

    getTransactions(): Observable<TransactionViewModel[]> {
        return (this.http.get(this.transactionsUrl).map(this.extractData).catch(this.handleError));
    }

    postTransaction(tvm: TransactionViewModel): Observable<TransactionViewModel> {
        let body = JSON.stringify({ tvm });
        let headers = new Headers({ 'Content-Type': 'application/json' });
        let options = new RequestOptions({ headers: headers });

        return (this.http.post(this.transactionsUrl, body, options)
            .map(this.extractData)
            .catch(this.handleError));
    }

    private extractData(res: Response) {
        let body = res.json();
        return body || [];
    }

    private handleError(error: any) {
        let errMsg = (error.message) ? error.message :
            error.status ? `${error.status} - ${error.statusText}` : 'Server error';
        console.error(errMsg);
        return Observable.throw(errMsg);
    }
}